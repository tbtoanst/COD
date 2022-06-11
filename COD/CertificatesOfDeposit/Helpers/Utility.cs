using AutoMapper;
using Dapper;
using CertificatesOfDeposit.Consts;
using CertificatesOfDeposit.Core.Configuration;
using CertificatesOfDeposit.Models.Account;
using CertificatesOfDeposit.Models.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;

namespace CertificatesOfDeposit.Helpers
{
    public static class Utility
    {
        public static T DeserializeObject<T>(this string str)
        {
            if (str == null)
                str = string.Empty;
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
        }
        public static string SerializeObject(this object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
        public static IEnumerable<IDictionary<string, object>> ToDict(this IEnumerable<dynamic> datas)
        {
            return datas.Select(s => (IDictionary<string, object>)s);
        }
        public static bool IsPropExist(dynamic obj, string property)
        {
            return ((Type)obj.GetType()).GetProperties().Where(p => p.Name.Equals(property)).Any();
        }
        public static void RegisterTypeMaps()
        {
            var mappedTypes = Assembly.GetAssembly(typeof(Initiator)).GetTypes().Where(
                f =>
                f.GetProperties().Any(
                    p =>
                    p.GetCustomAttributes(false).Any(
                        a => a.GetType().Name == ColumnAttributeTypeMapper<dynamic>.ColumnAttributeName)));

            var mapper = typeof(ColumnAttributeTypeMapper<>);
            foreach (var mappedType in mappedTypes)
            {
                var genericType = mapper.MakeGenericType(new[] { mappedType });
                SqlMapper.SetTypeMap(mappedType, Activator.CreateInstance(genericType) as SqlMapper.ITypeMap);
            }
        }
        public static T MapProp<T>(this object item)
        {
            return item.SerializeObject().DeserializeObject<T>();
        }
        public static D MapProp<T, D>(this T item)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<T, D>();
            });

            var mapper = configuration.CreateMapper();
            return mapper.Map<T, D>(item);
        }
        public static T GetValue<T>(this object item, string property)
        {
            Dictionary<string, object> obj;
            if (item is string)
                obj = ((string)item).DeserializeObject<Dictionary<string, object>>();
            else
            {
                var json = JsonConvert.SerializeObject(item);
                obj = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            }
            if (obj.ContainsKey(property))
                return obj[property].ToString().DeserializeObject<T>();
            return default(T);
        }
        public static IEnumerable<GroupResult<TElement>> GroupByMany<TElement>(
            this IEnumerable<TElement> elements,
            params Func<TElement, object>[] groupSelectors)
        {
            if (groupSelectors.Length > 0)
            {
                var selector = groupSelectors.First();

                //reduce the list recursively until zero
                var nextSelectors = groupSelectors.Skip(1).ToArray();
                return
                    elements.GroupBy(selector).Select(
                        g => new GroupResult<TElement>
                        {
                            Key = g.Key,
                            Count = g.Count(),
                            Item = g,
                            SubGroups = g.GroupByMany(nextSelectors)
                        });
            }
            return null;
        }
        public static SessionModel GetSession(this ClaimsPrincipal user)
        {
            var userId = user.Claims.FirstOrDefault(f => f.Type == ClaimTypes.Sid).Value;
            var userName = user.Claims.FirstOrDefault(f => f.Type == ClaimTypes.NameIdentifier).Value;
            var userFullName = user.Claims.FirstOrDefault(f => f.Type == ClaimTypes.Name).Value;
            var userEmail = user.Claims.FirstOrDefault(f => f.Type == ClaimTypes.Email).Value;
            var deptId = user.Claims.FirstOrDefault(f => f.Type == ClaimTypesCustom.DEPARTMENT_ID).Value;
            var deptCode = user.Claims.FirstOrDefault(f => f.Type == ClaimTypesCustom.DEPARTMENT_CODE).Value;
            var deptName = user.Claims.FirstOrDefault(f => f.Type == ClaimTypesCustom.DEPARTMENT_NAME).Value;
            var branchId = user.Claims.FirstOrDefault(f => f.Type == ClaimTypesCustom.BRANCH_ID).Value;
            var branchCode = user.Claims.FirstOrDefault(f => f.Type == ClaimTypesCustom.BRANCH_CODE).Value;
            var branchName = user.Claims.FirstOrDefault(f => f.Type == ClaimTypesCustom.BRANCH_NAME).Value;
            var branchCodeFCC = user.Claims.FirstOrDefault(f => f.Type == ClaimTypesCustom.BRANCH_CODE_FCC).Value;
            var branchNameFCC = user.Claims.FirstOrDefault(f => f.Type == ClaimTypesCustom.BRANCH_NAME_FCC).Value;
            var roles = user.Claims.Where(f => f.Type == ClaimTypes.Role).Select(s=>s.Value).ToList();

            return new SessionModel
            {
                UserId = userId,
                UserName = userName,
                UserFullName = userFullName,
                DeptpartmentId = deptId,
                DeptpartmentCode = deptCode,
                DeptpartmentName = deptName,
                BranchId = branchId,
                BranchCode = branchCode,
                BranchName = branchName,
                Roles = roles,
                BranchCodeFCC = branchCodeFCC,
                BranchNameFCC = branchNameFCC
            };
        }
    }
}
