using System;

using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;

namespace Vilinski.ReSharperPlugIn
{
    [DaemonStage(StagesBefore = new[] {typeof (LanguageSpecificDaemonStage)})]
    public class MakeMethodVirtualDaemonStage : IDaemonStage
    {
    	public IDaemonStageProcess CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind)
    	{
			return new MakeMethodVirtualDaemonStageProcess(process);
    	}

    	public ErrorStripeRequest NeedsErrorStripe(IPsiSourceFile sourceFile, IContextBoundSettingsStore settingsStore)
    	{
			return ErrorStripeRequest.STRIPE_AND_ERRORS;
    	}
    }
}