// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Rendering.Views;
using Microsoft.DotNet.Tools.Uninstall.Shared.BundleInfo;
using Microsoft.DotNet.Tools.Uninstall.Shared.BundleInfo.Versioning;
using Microsoft.VisualBasic.FileIO;

namespace Microsoft.DotNet.Tools.Uninstall.Shared.Configs;

internal abstract class BundleTypePrintInfo
{
    public abstract BundleType Type { get; }

    public string Header { get; }
    public Func<IDictionary<Bundle, string>, bool, GridView> GridViewGenerator { get; }
    public Option<bool> Option { get; }

    protected BundleTypePrintInfo(string header, Func<IDictionary<Bundle, string>, bool, GridView> gridViewGenerator, Option<bool> option)
    {
        ArgumentNullException.ThrowIfNull(header);
        ArgumentNullException.ThrowIfNull(gridViewGenerator);
        ArgumentNullException.ThrowIfNull(option);

        Header = header;
        GridViewGenerator = gridViewGenerator;
        Option = option;
    }

    public abstract IEnumerable<Bundle> Filter(IEnumerable<Bundle> bundles);
}

internal class BundleTypePrintInfo<TBundleVersion> : BundleTypePrintInfo
    where TBundleVersion : BundleVersion, IComparable<TBundleVersion>, new()
{
    public override BundleType Type => new TBundleVersion().Type;

    public BundleTypePrintInfo(string header, Func<IDictionary<Bundle, string>, bool, GridView> gridViewGenerator, Option<bool> option) :
        base(header, gridViewGenerator, option)
    { }

    public override IEnumerable<Bundle> Filter(IEnumerable<Bundle> bundles)
    {
        return Bundle<TBundleVersion>.FilterWithSameBundleType(bundles);
    }
}
