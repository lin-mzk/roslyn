﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CodeAnalysis.FindUsages;
using Microsoft.VisualStudio.Shell.TableManager;

namespace Microsoft.VisualStudio.LanguageServices.FindUsages
{
    internal partial class StreamingFindUsagesPresenter
    {
        private class ExternalReferenceItemEntry : Entry, ISupportsNavigation
        {
            private readonly ExternalReferenceItem _reference;

            public ExternalReferenceItemEntry(
                RoslynDefinitionBucket bucket,
                ExternalReferenceItem reference)
                : base(bucket)
            {
                _reference = reference;
            }

            public bool TryNavigateTo(bool isPreview)
                => _reference.TryNavigateTo(isPreview);

            protected override object GetValueWorker(string keyName)
                => keyName switch
                {
                    StandardTableKeyNames.DocumentName => _reference.DisplayPath,
                    StandardTableKeyNames.Line => _reference.Span.Start.Line,
                    StandardTableKeyNames.Column => _reference.Span.Start.Character,
                    StandardTableKeyNames.ProjectName => _reference.ProjectName,
                    StandardTableKeyNames.Text => _reference.Text,
                    _ => null,
                };
        }
    }
}
