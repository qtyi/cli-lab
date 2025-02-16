// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Linq;
using Microsoft.DotNet.Tools.Uninstall.MacOs;
using Microsoft.DotNet.Tools.Uninstall.Shared.BundleInfo;
using Microsoft.DotNet.Tools.Uninstall.Shared.Commands;
using Microsoft.DotNet.Tools.Uninstall.Shared.Configs.Verbosity;
using Microsoft.DotNet.Tools.Uninstall.Shared.Exceptions;
using Microsoft.DotNet.Tools.Uninstall.Shared.Utils;
using Microsoft.DotNet.Tools.Uninstall.Windows;

namespace Microsoft.DotNet.Tools.Uninstall.Shared.Configs;

internal static class CommandLineConfigs
{
    public static Parser UninstallCommandParser;

    private static readonly string ListCommandName = "list";
    private static readonly string DryRunCommandName = "dry-run";
    private static readonly string WhatIfCommandName = "whatif";
    private static readonly string RemoveCommandName = "remove";

    public static readonly RootCommand UninstallRootCommand = new(
        RuntimeInfo.RunningOnWindows ? LocalizableStrings.UninstallNoOptionDescriptionWindows 
        : LocalizableStrings.UninstallNoOptionDescriptionMac);

    public static readonly Command ListCommand = new(
        ListCommandName,
        LocalizableStrings.ListCommandDescription);

    public static readonly Command DryRunCommand = new(
        DryRunCommandName,
        LocalizableStrings.DryRunCommandDescription);

    public static readonly Command RemoveCommand = new(
       RemoveCommandName,
        LocalizableStrings.RemoveCommandDescription);


    public static readonly string SdkOptionName = "sdk";
    public static readonly string RuntimeOptionName = "runtime";
    public static readonly string AspNetRuntimeOptionName = "aspnet-runtime";
    public static readonly string HostingBundleOptionName = "hosting-bundle";
    public static readonly string X64OptionName = "x64";
    public static readonly string X86OptionName = "x86";

    public static readonly Option<bool> UninstallAllOption = new(
        "--all",
        LocalizableStrings.UninstallAllOptionDescription);

    public static readonly Option<bool> UninstallAllLowerPatchesOption = new(
        "--all-lower-patches",
        LocalizableStrings.UninstallAllLowerPatchesOptionDescription);

    public static readonly Option<bool> UninstallAllButLatestOption = new(
        "--all-but-latest",
        LocalizableStrings.UninstallAllButLatestOptionDescription);

    public static readonly Option<IEnumerable<string>> UninstallAllButOption = new(
        "--all-but",
        LocalizableStrings.UninstallAllButOptionDescription)
    {
        ArgumentHelpName = LocalizableStrings.UninstallAllButOptionArgumentName
    };

    public static readonly Option<string> UninstallAllBelowOption = new(
        "--all-below",
        LocalizableStrings.UninstallAllBelowOptionDescription)
    {
        ArgumentHelpName = LocalizableStrings.UninstallAllBelowOptionArgumentName
    };

    public static readonly Option<bool> UninstallAllPreviewsOption = new(
        "--all-previews",
        LocalizableStrings.UninstallAllPreviewsOptionDescription);

    public static readonly Option<bool> UninstallAllPreviewsButLatestOption = new(
        "--all-previews-but-latest",
        LocalizableStrings.UninstallAllPreviewsButLatestOptionDescription);

    public static readonly Option<string> UninstallMajorMinorOption = new(
        "--major-minor",
        LocalizableStrings.UninstallMajorMinorOptionDescription)
    {
        ArgumentHelpName = LocalizableStrings.UninstallMajorMinorOptionArgumentName
    };

    public static readonly Option<string> VerbosityOption = new(
        ["--verbosity", "-v"],
        LocalizableStrings.VerbosityOptionDescription)
    {
        ArgumentHelpName = LocalizableStrings.VerbosityOptionArgumentName
    };

    public static readonly Option<bool> ListX64Option = new(
        $"--{X64OptionName}",
        LocalizableStrings.ListX64OptionDescription);

    public static readonly Option<bool> ListX86Option = new(
        $"--{X86OptionName}",
        LocalizableStrings.ListX86OptionDescription);

    public static readonly Option<bool> VersionOption = new("--version")
    {
        IsHidden = true
    };

    public static readonly Option<bool> YesOption = new(
        ["--yes", "-y"],
        LocalizableStrings.YesOptionDescription);

    public static readonly Option<bool> ForceOption = new(
        "--force",
        RuntimeInfo.RunningOnWindows ? LocalizableStrings.ForceOptionDescriptionWindows
        : LocalizableStrings.ForceOptionDescriptionMac);

    public static readonly Option[] UninstallFilterBundlesOptions =
    [
        UninstallAllOption,
        UninstallAllLowerPatchesOption,
        UninstallAllButLatestOption,
        UninstallAllButOption,
        UninstallAllBelowOption,
        UninstallAllPreviewsOption,
        UninstallAllPreviewsButLatestOption,
        UninstallMajorMinorOption
    ];

    public static readonly Option<bool> ListSdkOption = new($"--{SdkOptionName}", LocalizableStrings.ListSdkOptionDescription);
    public static readonly Option<bool> ListRuntimeOption = new($"--{RuntimeOptionName}", LocalizableStrings.ListRuntimeOptionDescription);
    public static readonly Option<bool> ListAspNetRuntimeOption = new($"--{AspNetRuntimeOptionName}", LocalizableStrings.ListAspNetRuntimeOptionDescription);
    public static readonly Option<bool> ListHostingBundleOption = new($"--{HostingBundleOptionName}", LocalizableStrings.ListHostingBundleOptionDescription);

    public static readonly Option[] ListBundleTypeOptions =
    [
        ListSdkOption,
        ListRuntimeOption,
        ListAspNetRuntimeOption,
        ListHostingBundleOption
    ];

    public static readonly Option[] UninstallBundleTypeOptions =
    [
        new Option<bool>($"--{SdkOptionName}", LocalizableStrings.UninstallSdkOptionDescription),
        new Option<bool>($"--{RuntimeOptionName}", LocalizableStrings.UninstallRuntimeOptionDescription),
        new Option<bool>($"--{AspNetRuntimeOptionName}", LocalizableStrings.UninstallAspNetRuntimeOptionDescription),
        new Option<bool>($"--{HostingBundleOptionName}", LocalizableStrings.UninstallHostingBundleOptionDescription)
    ];

    public static readonly Option[] ArchUninstallOptions =
    [
        new Option<bool>($"--{X64OptionName}", LocalizableStrings.UninstallX64OptionDescription),
        new Option<bool>($"--{X86OptionName}", LocalizableStrings.UninstallX86OptionDescription)
    ];

    public static readonly Option[] AdditionalUninstallOptions =
    [
        VerbosityOption,
        VersionOption, 
        ForceOption
    ];

    public static readonly Dictionary<string, VerbosityLevel> VerbosityLevels = new()
    {
        { "q", VerbosityLevel.Quiet }, { "quiet", VerbosityLevel.Quiet },
        { "m", VerbosityLevel.Minimal }, { "minimal", VerbosityLevel.Minimal },
        { "n", VerbosityLevel.Normal }, { "normal", VerbosityLevel.Normal },
        { "d", VerbosityLevel.Detailed }, { "detailed", VerbosityLevel.Detailed },
        { "diag", VerbosityLevel.Diagnostic }, { "diagnostic", VerbosityLevel.Diagnostic }
    };

    public static ParseResult CommandLineParseResult;
    public static readonly IEnumerable<Option> RemoveAuxOptions;
    public static readonly IEnumerable<Option> DryRunAuxOptions;
    public static readonly IEnumerable<Option> WhatIfAuxOptions;
    public static readonly IEnumerable<Option> ListAuxOptions;

    static CommandLineConfigs() 
    {
        DryRunCommand.AddAlias(WhatIfCommandName);

        UninstallRootCommand.AddCommand(ListCommand);
        UninstallRootCommand.AddCommand(DryRunCommand);
        UninstallRootCommand.AddCommand(RemoveCommand);

        var supportedBundleTypeNames = SupportedBundleTypeConfigs.GetSupportedBundleTypes().Select(type => type.Option.Name);

        RemoveAuxOptions = UninstallBundleTypeOptions
            .Where(option => supportedBundleTypeNames.Contains(option.Name))
            .Concat(AdditionalUninstallOptions)
            .Append(YesOption);
        if (RuntimeInfo.RunningOnWindows)
        {
            RemoveAuxOptions = RemoveAuxOptions.Concat(ArchUninstallOptions);
        }
        AssignOptionsToCommand(RemoveCommand, RemoveAuxOptions
            .Concat(UninstallFilterBundlesOptions), true);

        DryRunAuxOptions = UninstallBundleTypeOptions
            .Where(option => supportedBundleTypeNames.Contains(option.Name))
            .Concat(AdditionalUninstallOptions);
        if (RuntimeInfo.RunningOnWindows)
        {
            DryRunAuxOptions = DryRunAuxOptions.Concat(ArchUninstallOptions);
        }
        AssignOptionsToCommand(DryRunCommand, DryRunAuxOptions
            .Concat(UninstallFilterBundlesOptions), true);

        ListAuxOptions = ListBundleTypeOptions
            .Where(option => supportedBundleTypeNames.Contains(option.Name))
            .Append(VerbosityOption);
        if (RuntimeInfo.RunningOnWindows)
        {
            ListAuxOptions = ListAuxOptions
                .Append(ListX64Option)
                .Append(ListX86Option);
        }
        AssignOptionsToCommand(ListCommand, ListAuxOptions);

        IBundleCollector bundleCollector = RuntimeInfo.RunningOnWindows ? new RegistryQuery() : new FileSystemExplorer();
        ListCommand.SetHandler(ExceptionHandler.HandleException(() => ListCommandExec.Execute(bundleCollector)));
        DryRunCommand.SetHandler(ExceptionHandler.HandleException(() => DryRunCommandExec.Execute(bundleCollector)));
        RemoveCommand.SetHandler(ExceptionHandler.HandleException(() => UninstallCommandExec.Execute(bundleCollector)));

        UninstallCommandParser = new CommandLineBuilder(UninstallRootCommand)
            .UseDefaults()
            .UseHelpBuilder(_ => new UninstallHelpBuilder())
            .Build();
        CommandLineParseResult = UninstallCommandParser.Parse(Environment.GetCommandLineArgs());
    }

    public static Option GetUninstallMainOption(this CommandResult commandResult)
    {
        var specified = UninstallFilterBundlesOptions
            .Where(option => commandResult.FindResultFor(option) != null);

        if (specified.Count() > 1)
        {
            throw new OptionsConflictException(specified);
        }

        var specifiedOption = specified.FirstOrDefault();

        if (specifiedOption != null && commandResult.Tokens.Count > 0)
        {
            var optionName = $"--{specifiedOption.Name}";

            if (specifiedOption.Name.Equals(UninstallAllButOption.Name))
            {
                throw new VersionBeforeOptionException(optionName);
            }
            else if (specifiedOption.Name.Equals(UninstallAllBelowOption.Name) || specifiedOption.Name.Equals(UninstallMajorMinorOption.Name))
            {
                throw new MoreThanOneVersionSpecifiedException(optionName);
            }
            else
            {
                throw new MoreThanZeroVersionSpecifiedException(optionName);
            }
        }

        return specifiedOption;
    }

    public static BundleType GetTypeSelection(this ParseResult parseResult)
    {
        var supportedBundleTypes = SupportedBundleTypeConfigs.GetSupportedBundleTypes();

        var typeSelection = supportedBundleTypes
            .Where(type => parseResult.GetValueForOption<bool>(type.Option))
            .Select(type => type.Type)
            .Aggregate((BundleType)0, (orSum, next) => orSum | next);

        return typeSelection == 0 ?
            supportedBundleTypes.Select(type => type.Type).Aggregate((BundleType)0, (orSum, next) => orSum | next) :
            typeSelection;
    }

    public static BundleArch GetArchSelection(this ParseResult parseResult)
    {
        var archSelection = new[]
        {
            (Option: ListX64Option, Arch: BundleArch.X64),
            (Option: ListX86Option, Arch: BundleArch.X86)
        }
        .Where(tuple => parseResult.GetValueForOption<bool>(tuple.Option))
        .Select(tuple => tuple.Arch)
        .Aggregate((BundleArch)0, (orSum, next) => orSum | next);

        return archSelection == 0 ?
            archSelection = Enum.GetValues(typeof(BundleArch)).OfType<BundleArch>().Aggregate((BundleArch)0, (orSum, next) => orSum | next) :
            archSelection;
    }

    public static VerbosityLevel GetVerbosityLevel(this CommandResult commandResult)
    {
        var optionResult = commandResult.FindResultFor(VerbosityOption);

        if (optionResult == null)
        {
            return VerbosityLevel.Normal;
        }

        var levelString = optionResult.GetValueOrDefault<string>();

        if (VerbosityLevels.TryGetValue(levelString, out var level))
        {
            return level;
        }
        else
        {
            throw new VerbosityLevelInvalidException();
        }
    }

    private static void AssignOptionsToCommand(Command command, IEnumerable<Option> options, bool addVersionArgument = false)
    {
        foreach (var option in options
            .OrderBy(option => option.Name))
        {
            command.AddOption(option);
        }
        if (addVersionArgument)
        {
            command.AddArgument(new Argument<IEnumerable<string>>
            {
                Name = LocalizableStrings.UninstallNoOptionArgumentName,
                Description = LocalizableStrings.UninstallNoOptionArgumentDescription
            });
        }
    }
}
