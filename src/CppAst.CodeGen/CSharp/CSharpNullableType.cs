﻿// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using CppAst.CodeGen.Common;

namespace CppAst.CodeGen.CSharp
{
    public class CSharpNullableType : CSharpTypeWithElement
    {
        public CSharpNullableType(CSharpType elementType) : base(elementType)
        {
        }

        public override void DumpTo(CodeWriter writer)
        {
            ElementType?.DumpReferenceTo(writer);
            writer.Write("?");
        }
    }
}