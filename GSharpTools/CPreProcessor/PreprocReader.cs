using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace GSharpTools.CPreProcessor
{
    internal class PreprocReader
    {
        private readonly CPreProcessor Processor;

        public PreprocReader(CPreProcessor processor)
        {
            Processor = processor;
        }

        public bool Feed(string filename)
        {
            Filename = filename;
            using (StreamReader reader = File.OpenText(filename))
            {
                Data = reader.ReadToEnd();
                reader.Close();
            }
            return Parse();
        }

        private bool Parse()
        {
            Line = 0;
            Column = 1;
            StopSignal = false;
            IsRecordingPreProcMacro = false;
            IfDefStackPosition = -1;
            MacroText = "";

            char c;

            for (ReadIndex = 0; !StopSignal && (ReadIndex < Data.Length); ++ReadIndex)
            {
                c = Data[ReadIndex];
                switch (c)
                {
                    case '\n':
                        if ((ReadIndex > 0) && (Data[ReadIndex - 1] != '\r'))
                            OnNewline();
                        else
                            --Column;
                        break;

                    case '\r':
                        OnNewline();
                        break;

                    case '#':
                        OnHashSign();
                        break;


                }
                ++Column;
            }
            return true;
        }

        private void OnNewline()
        {
            Column = 0;
            ++Line;
            StopRecordingMacro();
        }

        private void OnCarriageReturn()
        {
            Column = 0;
            ++Line;
            StopRecordingMacro();
        }

        private void StopRecordingMacro()
        {
            if (IsRecordingPreProcMacro)
            {
                if (MacroText == "")
                {
                    MacroText = Data.Substring(StartOfMacro, ReadIndex - StartOfMacro);
                }
                else
                {
                    MacroText = MacroText + Data.Substring(StartOfMacro, ReadIndex - StartOfMacro);
                }

                if (MacroText.EndsWith("\\"))
                {
                    if (Data[ReadIndex] == '\r')
                        ++ReadIndex;

                    MacroText = MacroText.Remove(MacroText.Length - 1);
                    StartOfMacro = ReadIndex + 1;
                    return;
                }

                IsRecordingPreProcMacro = false;

                // better: tokenize whole line now, feed to classes
                Tokens = new PreprocTokenizer(MacroText);

                if (IsLineIncluded())
                {
                    if (MacroText.StartsWith("#define"))
                    {
                        PreProc_Define();
                    }
                    else if (MacroText.StartsWith("#ifndef"))
                    {
                        PreProc_Ifndef();
                    }
                    else if (MacroText.StartsWith("#ifdef"))
                    {
                        PreProc_Ifdef();
                    }
                    else if (MacroText.StartsWith("#else"))
                    {
                        PreProc_Else();
                    }
                    else if (MacroText.StartsWith("#endif"))
                    {
                        PreProc_Endif();
                    }
                    else if (MacroText.StartsWith("#if"))
                    {
                        PreProc_If();
                    }
                    else if (MacroText.StartsWith("#elif"))
                    {
                        PreProc_ElseIf();
                    }
                    else if (MacroText.StartsWith("#undef"))
                    {
                        PreProc_ElseIf();
                    }
                    else if (MacroText.StartsWith("#include"))
                    {
                        PreProc_Include();
                    }
                    else if (MacroText.StartsWith("#pragma"))
                    {
                        PreProc_Pragma();
                    }
                    else if (MacroText.StartsWith("#line"))
                    {
                        // do nothing
                    }
                    else if (MacroText.StartsWith("#error"))
                    {
                        // do nothing
                    }
                    else if ((Tokens.Count > 2) && (Tokens[1] == "define"))
                    {
                        // do nothing
                    }
                    else
                    {
                        Trace.TraceError("unknown preproc cmd: <{0}>", MacroText);
                    }
                }
                else
                {
                    if (MacroText.StartsWith("#ifndef"))
                    {
                        PreProc_Ifndef();
                    }
                    else if (MacroText.StartsWith("#ifdef"))
                    {
                        PreProc_Ifdef();
                    }
                    else if (MacroText.StartsWith("#else"))
                    {
                        PreProc_Else();
                    }
                    else if (MacroText.StartsWith("#endif"))
                    {
                        PreProc_Endif();
                    }
                    else if (MacroText.StartsWith("#if"))
                    {
                        PreProc_If();
                    }
                    else if (MacroText.StartsWith("#elif"))
                    {
                        PreProc_ElseIf();
                    }
                    else
                    {
                        Trace.TraceInformation("Ignore '{0}' in line {1}", MacroText, Line);
                    }
                }
                MacroText = "";
            }
        }

        private bool IsWhitespace(char c)
        {
            return (c == ' ') || (c == '\t') || (c == '\r') || (c == '\n');
        }

        private void PreProc_Include()
        {
            string IncludeName = null;
            int StartOfInclude = -1;
            int index = 8;
            char ExpectedEndOfIncludeChar = '\0';

            // identify start of #include name. somebody could insert *a lot* of spaces before the <filename> bit
            while (index < MacroText.Length)
            {
                char c = MacroText[index];

                if ((c == ExpectedEndOfIncludeChar) && (StartOfInclude >= 0))
                {
                    IncludeName = MacroText.Substring(StartOfInclude, index - StartOfInclude);
                    break;
                }
                else if (!IsWhitespace(c))
                {
                    if (StartOfInclude < 0)
                    {
                        if (c == '<')
                            ExpectedEndOfIncludeChar = '>';
                        else
                            ExpectedEndOfIncludeChar = c;

                        StartOfInclude = index + 1;
                    }
                }
                ++index;
            }

            //Trace.TraceInformation("=> include '{0}'.", IncludeName);

            string incpath = Processor.Includes.Locate(IncludeName);
            if (incpath != null)
            {
                Processor.Feed(incpath);
            }
            else
            {
                Trace.TraceInformation("FATAL ERROR, unable to find include file '{0}'", IncludeName);
            }
        }

        private void PreProc_Define()
        {
            int StartOfMacroName = -1;
            int index = 7;
            int EndOfMacroName = -1;
            while (index < MacroText.Length)
            {
                char c = MacroText[index];

                if (!IsWhitespace(c))
                {
                    StartOfMacroName = index;
                    break;
                }
                ++index;
            }

            bool lockForClosingBracket = false;
            bool hasArguments = false;

            while (index < MacroText.Length)
            {
                char c = MacroText[index];

                if (IsWhitespace(c))
                {
                    if (!lockForClosingBracket)
                    {
                        EndOfMacroName = index;
                        break;
                    }
                }
                else if (lockForClosingBracket)
                {
                    if (c == ')')
                    {
                        ++index;
                        break;
                    }
                }
                else if (c == '(')
                {
                    lockForClosingBracket = true;
                    hasArguments = true;
                    EndOfMacroName = index;
                }

                ++index;
            }
            if (EndOfMacroName > StartOfMacroName)
            {
                string MacroName = MacroText.Substring(StartOfMacroName, EndOfMacroName - StartOfMacroName);
                string MacroArgs = null;
                if (hasArguments)
                    MacroArgs = MacroText.Substring(EndOfMacroName + 1, index - EndOfMacroName - 2);
                string MacroDefinition = MacroText.Substring(index).Trim();

                //Trace.TraceInformation("=> define '{0}' [{2}] as '{1}'.", MacroName,MacroDefinition, MacroArgs);

                MacroDefinition = Evaluate(MacroDefinition);

                //Trace.TraceInformation("=> evaluated as '{0}'", MacroDefinition);
                Processor.PreProcMacros[MacroName] = new PreprocMacro(hasArguments, MacroName, MacroArgs, MacroDefinition);
            }
            else
            {
                string MacroName = MacroText.Substring(StartOfMacroName);
                //Trace.TraceInformation("=> define '{0}'.", MacroName);
                Processor.PreProcMacros[MacroName] = new PreprocMacro(false, MacroName, null, "");
            }
        }

        private string Evaluate(string text)
        {
            PreprocTokenizer preproc = new PreprocTokenizer(text);
            for (int index = 0; index < preproc.Count; ++index)
            {
                string token = preproc[index];
                if (Processor.PreProcMacros.ContainsKey(token))
                {
                    PreprocMacro macro = Processor.PreProcMacros[token];
                    if (macro.HasArguments)
                    {
                        // check to see if the arguments are found
                        Dictionary<string, string> ArgumentValues = new Dictionary<string, string>();
                        int nNextIndex = ExtractArguments(macro, preproc, index + 2, ArgumentValues);
                        if (nNextIndex > 0)
                        {
                            while (nNextIndex >= index)
                            {
                                preproc.RemoveAt(nNextIndex--);
                            }

                            // insert definition of macro, replacing the arguments as specified
                            macro.Expand(preproc, index, ArgumentValues);
                        }
                        else
                        {
                            Trace.TraceError("Warning: Unable to expand macro.\r\nText = '{0}'.\r\nFile = {1} [Line {2}]", text, Filename, Line);
                            return text;
                        }
                    }
                    else
                    {
                        preproc[index] = macro.Definition;
                    }
                }
            }
            return preproc.Join();
        }

        private int ExtractArguments(PreprocMacro macro, List<string> Tokens, int StartIndex, Dictionary<string, string> ArgumentValues)
        {
            int TokenIndex = 0;
            while (StartIndex < Tokens.Count)
            {
                string token = Tokens[StartIndex];
                if (token == ")")
                {
                    return StartIndex;
                }
                else if (token != ",")
                {
                    if (TokenIndex < macro.Arguments.Count)
                    {
                        ArgumentValues[macro.Arguments[TokenIndex++]] = Tokens[StartIndex];
                    }
                    else
                    {
                        return -1;
                    }
                }
                ++StartIndex;
            }
            return -1;
        }

        private void PreProc_Ifndef()
        {
            string MacroName = MacroText.Substring(8);
            IfDefStack[++IfDefStackPosition] = !Processor.PreProcMacros.ContainsKey(MacroName);
        }

        private void PreProc_Ifdef()
        {
            string MacroName = MacroText.Substring(6);




            IfDefStack[++IfDefStackPosition] = Processor.PreProcMacros.ContainsKey(MacroName);
        }

        private void PreProc_Else()
        {
            if (IfDefStackPosition >= 0)
            {
                IfDefStack[IfDefStackPosition] = !IfDefStack[IfDefStackPosition];
            }
        }

        private void PreProc_Endif()
        {
            if (IfDefStackPosition >= 0)
            {
                --IfDefStackPosition;
            }
        }

        private void PreProc_If()
        {
            Trace.TraceError("IGNORE: {0}", MacroText);

            string MacroDefinition = Evaluate(MacroText);
            IfDefStack[++IfDefStackPosition] = false;
        }

        private void PreProc_ElseIf()
        {
            Trace.TraceError("IGNORE: {0}", MacroText);
            IfDefStack[++IfDefStackPosition] = false;
        }

        private bool IsLineIncluded()
        {
            if (IfDefStackPosition >= 0)
                return IfDefStack[IfDefStackPosition];
            else
                return true;
        }

        private void OnHashSign()
        {
            if (Column == 1)
            {
                IsRecordingPreProcMacro = true;
                StartOfMacro = ReadIndex;
            }
        }

        private void PreProc_Pragma()
        {
            PreprocTokenizer tokens = new PreprocTokenizer(MacroText);
            if ((tokens.Count > 1) && (tokens[1] == "once"))
            {
                Processor.ReadOnceList.Add(Filename);
            }
        }

        private bool[] IfDefStack = new bool[10240];
        private int IfDefStackPosition;
        private string MacroText;
        private bool StopSignal;
        private string Data;
        private int Line;
        private int Column;
        private int ReadIndex;
        private bool IsRecordingPreProcMacro;
        private int StartOfMacro;
        private string Filename;
        private PreprocTokenizer Tokens;
    }
}
