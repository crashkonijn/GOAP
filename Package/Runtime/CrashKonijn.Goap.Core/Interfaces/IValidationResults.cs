using System.Collections.Generic;

namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IValidationResults
    {
        void AddError(string error);
        void AddWarning(string warning);
        List<string> GetErrors();
        List<string> GetWarnings();
        bool HasErrors();
        bool HasWarnings();
    }
}