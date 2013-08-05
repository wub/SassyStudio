﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SassyStudio.Compiler.Parsing
{
    class MixinReference : ComplexItem
    {
        readonly List<FunctionArgument> _Arguments = new List<FunctionArgument>(0);

        public AtRule Rule { get; protected set; }
        public MixinName Name { get; protected set; }
        public TokenItem OpenBrace { get; protected set; }

        public TokenItem CloseBrace { get; protected set; }
        public TokenItem Semicolon { get; protected set; }
        public IReadOnlyCollection<FunctionArgument> Arguments { get { return _Arguments; } }

        public override bool Parse(IItemFactory itemFactory, ITextProvider text, ITokenStream stream)
        {
            if (AtRule.IsRule(text, stream, "include") && MixinName.IsValidName(stream.Peek(2)))
            {
                Rule = AtRule.CreateParsed(itemFactory, text, stream);
                Name = MixinName.CreateParsed(itemFactory, text, stream, SassClassifierType.MixinReference);

                Children.Add(Rule);
                Children.Add(Name);

                if (stream.Current.Type == TokenType.OpenFunctionBrace)
                {
                    OpenBrace = Children.AddCurrentAndAdvance(stream, SassClassifierType.FunctionBrace);

                    while (!IsTerminator(stream.Current.Type))
                    {
                        var argument = itemFactory.CreateSpecific<FunctionArgument>(this, text, stream);
                        if (argument != null && argument.Parse(itemFactory, text, stream))
                        {
                            _Arguments.Add(argument);
                            Children.Add(argument);
                        }
                        else
                        {
                            Children.AddCurrentAndAdvance(stream);
                        }
                    }

                    if (stream.Current.Type == TokenType.CloseFunctionBrace)
                        CloseBrace = Children.AddCurrentAndAdvance(stream, SassClassifierType.FunctionBrace);
                }

                if (stream.Current.Type == TokenType.Semicolon)
                    Semicolon = Children.AddCurrentAndAdvance(stream);
            }

            return Children.Count > 0;
        }

        public override void Freeze()
        {
            base.Freeze();
            _Arguments.TrimExcess();
        }

        static bool IsTerminator(TokenType type)
        {
            switch (type)
            {
                case TokenType.EndOfFile:
                case TokenType.CloseFunctionBrace:
                case TokenType.Semicolon:
                    return true;
                default:
                    return false;
            }
        }
    }
}
