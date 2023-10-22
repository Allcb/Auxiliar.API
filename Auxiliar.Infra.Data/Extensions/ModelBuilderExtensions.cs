using Auxiliar.Domain.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using System.Reflection;

namespace Auxiliar.Infra.Data.Extensions
{
    public static class ModelBuilderExtensions
    {
        #region Campos Privados

        private static readonly string DataProtectionKey = "DataProtectionKey";

        #endregion Campos Privados

        #region Metodos Publicos

        public static ModelBuilder ApplyGlobalStandards(this ModelBuilder builder)
        {
            foreach (IMutableEntityType entityType in builder.Model.GetEntityTypes())
            {
                if (!entityType.GetTableName().Contains('_'))
                    entityType.SetTableName(entityType.ClrType.Name);

                entityType.GetForeignKeys()
                          .Where(foreignkey => !foreignkey.IsOwnership && foreignkey.DeleteBehavior == DeleteBehavior.Cascade)
                          .ToList()
                          .ForEach(foreignkey => foreignkey.DeleteBehavior = DeleteBehavior.Restrict);

                if (entityType.ClrType.BaseType == typeof(Entity))
                {
                    ParameterExpression _parameter = Expression.Parameter(entityType.ClrType);

                    MethodInfo _propertyMethodInfo = typeof(EF).GetMethod("Property")
                                                               .MakeGenericMethod(typeof(bool));

                    MethodCallExpression _isDeletedProperty = Expression.Call(_propertyMethodInfo,
                                                                              _parameter,
                                                                              Expression.Constant(nameof(Entity.Excluido)));

                    BinaryExpression _compareExpression = Expression.MakeBinary(ExpressionType.Equal,
                                                                                _isDeletedProperty,
                                                                                Expression.Constant(false));

                    LambdaExpression _lambda = Expression.Lambda(_compareExpression,
                                                                 _parameter);

                    builder.Entity(entityType.ClrType)
                           .Property(nameof(Entity.DataCriacao))
                           .ValueGeneratedOnUpdate();

                    builder.Entity(entityType.ClrType)
                           .HasQueryFilter(_lambda);
                }

                foreach (IMutableProperty property in entityType.GetProperties())
                {
                    switch (property.Name)
                    {
                        case nameof(Entity.Id):
                            property.IsKey();
                            break;

                        case nameof(Entity.DataModificacao):
                            property.IsNullable = true;
                            break;

                        case nameof(Entity.DataCriacao):
                            property.IsNullable = false;
                            property.SetDefaultValueSql("GETDATE()");
                            property.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
                            break;

                        case nameof(Entity.Excluido):
                            property.IsNullable = false;
                            property.SetDefaultValue(false);
                            break;
                    }

                    if (property.ClrType == typeof(string) && string.IsNullOrEmpty(property.GetColumnType()))
                        DefinirTipoColuna(entityType, property);

                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                        property.SetColumnType("datetime");
                }
            }

            return builder;
        }

        #endregion Metodos Publicos

        private static void DefinirTipoColuna(IMutableEntityType tipoEntidade, IMutableProperty propriedade)

        {
            string _tipoPropriedade = !tipoEntidade.Name.Contains(DataProtectionKey)
                                          ? $"varchar({propriedade.GetMaxLength() ?? 100})"
                                          : "nvarchar(MAX)";

            propriedade.SetColumnType(_tipoPropriedade);
        }
    }
}