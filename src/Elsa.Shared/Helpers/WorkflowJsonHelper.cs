using System.IO;
using System.Threading.Tasks;

namespace Elsa.Shared
{

    public static class WorkflowJsonHelper
    {
        public static async Task<string> ReadJsonWorkflowResource(string resourceName)
        {
            var assembly = typeof(WorkflowJsonHelper).Assembly;
            var resourcePath = $"{typeof(WorkflowJsonHelper).Assembly.GetName().Name}.workflows.{resourceName}";
            using var reader = new StreamReader(assembly.GetManifestResourceStream(resourcePath));
            return await reader.ReadToEndAsync();
        }

    }
}