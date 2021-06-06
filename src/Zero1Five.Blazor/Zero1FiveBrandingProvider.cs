﻿using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Zero1Five.Blazor
{
    [Dependency(ReplaceServices = true)]
    public class Zero1FiveBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "Zero1Five";
    }
}
