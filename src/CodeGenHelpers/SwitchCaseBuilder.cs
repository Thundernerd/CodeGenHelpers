﻿using System;

namespace CodeGenHelpers
{
    public sealed class SwitchCaseBuilder : IBuilder
    {
        private SwitchBuilder _parent { get; }
        private string _case { get; }
        private Action<ICodeWriter> _content;

        public SwitchCaseBuilder(SwitchBuilder parent, string @case)
        {
            _parent = parent;
            _case = @case;
        }

        public SwitchBuilder WithContent(Action<ICodeWriter> contentDelegate)
        {
            _content = contentDelegate ?? EmptyBody;
            return _parent;
        }

        public SwitchBuilder WithExpression(string returnValue = "null")
        {
            _content = w => w.Append($"{_case} => {returnValue}");
            return _parent;
        }

        void IBuilder.Write(ref CodeWriter writer)
        {
            if (_parent.Expression)
            {
                _content?.Invoke(writer);
            }
            else
            {
                writer.AppendLine($"case {_case}:");
                writer.IncreaseIndent();
                _content?.Invoke(writer);
                writer.AppendLine("break;");
                writer.DecreaseIndent();
            }
        }

        private static void EmptyBody(ICodeWriter writer) { }
    }
}
