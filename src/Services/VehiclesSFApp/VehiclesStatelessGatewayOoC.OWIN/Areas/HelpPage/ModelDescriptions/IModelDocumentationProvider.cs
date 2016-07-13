using System;
using System.Reflection;

namespace VehiclesStatelessGatewayOoC.OWIN.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}