﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SassyStudio.Compiler.Parsing;

namespace SassyStudio.Editor.Intellisense
{
    class FunctionContainer : CompletionContainerBase
    {
        readonly UserFunctionDefinition Definition;

        public FunctionContainer(UserFunctionDefinition definition, ITextProvider text)
        {
            Definition = definition;
            Start = Math.Max(definition.Start, ((definition.Body != null && definition.Body.OpenCurlyBrace != null) ? definition.Body.OpenCurlyBrace.Start : 0));
            End = definition.End;

            foreach (var variable in definition.Arguments.Select(x => x.Variable))
                AddVariable(variable, text);
        }
    }
}
