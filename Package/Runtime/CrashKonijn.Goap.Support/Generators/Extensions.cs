using System.Linq;
using CrashKonijn.Goap.Scriptables;
using CrashKonijn.Goap.Support.Loaders;
using UnityEngine;

namespace CrashKonijn.Goap.Support.Generators
{
    public static class Extensions
    {
        public static ClassRefStatus GetStatus(this ClassRef classRef, Script[] scripts)
        {
            var (status, match) = classRef.GetMatch(scripts);
            
            return status;
        }

        public static (ClassRefStatus status, Script script) GetMatch(this ClassRef classRef, Script[] scripts)
        {
            if (classRef.name == "" && classRef.id == "")
            {
                return (ClassRefStatus.None, null);
            }
            
            // Full match
            if (scripts.Any(x => x.id == classRef.id && x.type.Name == classRef.name))
            {
                return (ClassRefStatus.Full, scripts.First(x => x.id == classRef.id && x.type.Name == classRef.name));
            }
            
            // Id Match
            if (scripts.Any(x => x.id == classRef.id))
            {
                return (ClassRefStatus.Id, scripts.First(x => x.id == classRef.id));
            }
            
            // Name Match
            if (scripts.Any(x => x.type.Name == classRef.name))
            {
                return (ClassRefStatus.Name, scripts.First(x => x.type.Name == classRef.name));
            }
            
            return (ClassRefStatus.None, null);
        }
    }
}