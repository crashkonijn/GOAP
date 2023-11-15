using System.Linq;
using CrashKonijn.Goap.Scriptables;
using CrashKonijn.Goap.Support.Loaders;

namespace CrashKonijn.Goap.Support.Generators
{
    public static class Extensions
    {
        public static ClassRefStatus GetStatus(this ClassRef classRef, Script[] scripts)
        {
            if (classRef.name == "" && classRef.id == "")
            {
                return ClassRefStatus.None;
            }
            
            // Full match
            if (scripts.Any(x => x.id == classRef.id && x.type.Name == classRef.name))
            {
                return ClassRefStatus.Full;
            }
            
            // Id Match
            if (scripts.Any(x => x.id == classRef.id))
            {
                return ClassRefStatus.Id;
            }
            
            // Name Match
            if (scripts.Any(x => x.type.Name == classRef.name))
            {
                return ClassRefStatus.Name;
            }
            
            return ClassRefStatus.None;
        }
    }
}