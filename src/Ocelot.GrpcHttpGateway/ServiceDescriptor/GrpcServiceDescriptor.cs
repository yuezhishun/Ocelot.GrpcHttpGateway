using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Ocelot.GrpcHttpGateway
{
    public class GrpcServiceDescriptor : IGrpcServiceDescriptor
    {
        protected ConcurrentDictionary<string, ConcurrentDictionary<string, MethodDescriptor>> CurrentService = new ConcurrentDictionary<string, ConcurrentDictionary<string, MethodDescriptor>>();
        protected string pattern = "^System|^mscorlib|^Microsoft|^Autofac|^EntityFramework|^Fluent|^Newtonsoft|^netstandard|^Npgsql|^NLog|^MySql|^Anonymously|^DynamicProxyGenAssembly";

        public GrpcServiceDescriptor()
        {
            LoadGrpcAssmbly();
        }

        public virtual ConcurrentDictionary<string, ConcurrentDictionary<string, MethodDescriptor>> GetGrpcDescript()
        {
            return CurrentService;
        }

        protected virtual void LoadGrpcAssmbly(string path = "")
        {
            if (string.IsNullOrEmpty(path))
                path = AppContext.BaseDirectory;
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            foreach (var file in Directory.GetFiles(path, "*.dll"))
            {
                assemblies.Add(Assembly.LoadFile(file));
            }
            foreach (var assembly in assemblies)
            {
                if (Regex.IsMatch(assembly.FullName, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled))
                    continue;
                GetGrpcDescript(assembly.GetTypes());
            }

        }

        protected virtual void GetGrpcDescript(Type[] types)
        {
            //根据类名特点查找FileDescriptor
            var fileTypes = types.Where(type => type.Name.Contains("Reflection"));
            foreach (var type in fileTypes)
            {
                BindingFlags flag = BindingFlags.Static | BindingFlags.Public;
                var property = type.GetProperties(flag).FirstOrDefault();
                if (property is null)
                    continue;
                var fileDescriptor = property.GetValue(null) as FileDescriptor;
                if (fileDescriptor is null)
                    continue;

                foreach (var svr in fileDescriptor.Services)
                {
                    if (CurrentService.ContainsKey(svr.Name.ToUpper()))
                        continue;
                    if (CurrentService.TryAdd(svr.Name.ToUpper(), new ConcurrentDictionary<string, MethodDescriptor>()))
                        foreach (var method in svr.Methods)
                        {
                            CurrentService[svr.Name.ToUpper()].TryAdd(method.Name.ToUpper(), method);
                        }
                }
            }
        }
    }
}
