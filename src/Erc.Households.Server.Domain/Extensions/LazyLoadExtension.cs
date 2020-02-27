using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Server.Domain.Extensions
{
    public static class LazyLoadExtension
    {
        public static TRelated Load<TRelated>(
            this Action<object, string> loader,
            object entity,
            ref TRelated navigationField,
            [System.Runtime.CompilerServices.CallerMemberName] string navigationName = null)
            where TRelated : class
        {
            loader?.Invoke(entity, navigationName);

            return navigationField;
        }
    }
}
