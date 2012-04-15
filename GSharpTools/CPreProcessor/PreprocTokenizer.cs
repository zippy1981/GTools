using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSharpTools.CPreProcessor
{
    public class PreprocTokenizer : List<string>
    {
        public PreprocTokenizer(string text)
        {
            Text = text;
            ReadIndex = 0;
            bool EscapeNextChar = false;
            while (ReadIndex < text.Length)
            {
                char c = text[ReadIndex];

                if (Type == RecordingType.BlockComment)
                {
                    if ((c == '*') && (PeekNextChar() == '/'))
                    {
                        ++ReadIndex;
                        IsRecording = false;
                        Type = RecordingType.Unknown;
                    }
                }
                else if (Type == RecordingType.String)
                {
                    if (EscapeNextChar)
                    {
                        EscapeNextChar = false;
                    }
                    else if (c == '\\')
                    {
                        EscapeNextChar = true;
                    }
                    else if (c == '"')
                    {
                        ++ReadIndex;
                        StopRecording();
                        --ReadIndex;
                    }
                }
                else
                {
                    // is separator
                    if ("(){}:%[]<>=+,-*/!?\\';".IndexOf(c) >= 0)
                    {
                        if ((c == '/') && (PeekNextChar() == '*'))
                        {
                            StartRecording(RecordingType.BlockComment);
                        }
                        else
                        {
                            if ("+-/<>/=".IndexOf(c) < 0)
                                StopRecording();

                            if( ReadIndex < text.Length )
                                StartRecording(RecordingType.Operator);
                        }
                    }
                    else if (c == '#')
                    {
                        // special case, because this could be a preproc macro argument :(
                        char d = PeekNextChar();
                        if ((d != '#') && (d != '\0') && (d != ' ') && (d != '\t'))
                        {
                            StopRecording();
                            StartRecording(RecordingType.Identifier);
                            ++ReadIndex;
                        }
                        else if (c == '#')
                        {
                            StopRecording();
                            StartRecording(RecordingType.Identifier);
                            ++ReadIndex;
                            ++ReadIndex;
                            StopRecording();
                            --ReadIndex;
                        }
                        else
                        {
                            StopRecording();
                            StartRecording(RecordingType.Identifier);
                        }
                    }
                    else if (" \t\r\n".IndexOf(c) >= 0)
                    {
                        StopRecording();
                    }
                    else if (c == '"')
                    {
                        StartRecording(RecordingType.String);
                    }
                    else
                    {
                        StartRecording(RecordingType.Identifier);
                    }
                }
                ++ReadIndex;
            }
            StopRecording();
        }

        private char PeekNextChar()
        {
            if( Text.Length > (ReadIndex+1) )
                return Text[ReadIndex + 1];
            return '\0';
        }

        public string Join()
        {
            return StringList.Join(this, " ");
        }

        private void StartRecording(RecordingType type)
        {
            if (IsRecording)
            {
                if (Type == type)
                    return;

                StopRecording();
            }
            if (ReadIndex < Text.Length)
            {
                StartIndex = ReadIndex;
                Type = type;
                IsRecording = true;
            }
        }


        private void StopRecording()
        {
            if (IsRecording)
            {
                string token = Text.Substring(StartIndex, ReadIndex - StartIndex);
                if (token == "//")
                {
                    ReadIndex = Text.Length;
                }
                else
                {
                    Add(token);
                }
                IsRecording = false;
            }
            Type = RecordingType.Unknown;
        }

        private enum RecordingType {
            Unknown,
            String,
            Identifier,
            Operator,
            BlockComment
        } 

        private bool IsRecording = false;
        private int StartIndex = 0;
        private RecordingType Type = RecordingType.Unknown;
        private string Text;
        private int ReadIndex = 0;
    }
}
