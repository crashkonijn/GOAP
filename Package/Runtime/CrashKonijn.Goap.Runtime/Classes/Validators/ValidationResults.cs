using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class ValidationResults : IValidationResults
    {
        private readonly string name;
        private readonly List<string> errors = new();
        private readonly List<string> warnings = new();

        public ValidationResults(string name)
        {
            this.name = name;
        }

        public void AddError(string error)
        {
            this.errors.Add($"[{this.name}] {error}");
        }

        public void AddWarning(string warning)
        {
            this.warnings.Add($"[{this.name}] {warning}");
        }

        public List<string> GetErrors()
        {
            return this.errors;
        }

        public List<string> GetWarnings()
        {
            return this.warnings;
        }

        public bool HasErrors()
        {
            return this.errors.Any();
        }

        public bool HasWarnings()
        {
            return this.warnings.Any();
        }
    }
}
