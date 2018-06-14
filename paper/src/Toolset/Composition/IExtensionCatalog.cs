//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Toolset.Composition
//{
//  public interface IExtensionCatalog : IDisposable
//  {
//    #region GetExtension

//    object GetExtension(Type contratType);

//    TExtension GetExtension<TExtension>();

//    #endregion

//    #region GetExtensions

//    object[] GetExtensions(Type contratType);

//    TExtension[] GetExtensions<TExtension>();

//    #endregion

//    #region GetExtensionTypes

//    Type[] GetExtensionTypes(Type contratType);

//    Type[] GetExtensionTypes<TContract>();

//    #endregion

//    #region GetMetadata

//    IDictionary<string, object> GetMetadata(Type extensionType);

//    IDictionary<string, object> GetMetadata(object extensionInstance);

//    TMetadata GetMetadata<TMetadata>(Type extensionType) where TMetadata : new();

//    TMetadata GetMetadata<TMetadata>(object extensionInstance) where TMetadata : new();

//    #endregion

//    #region FillMetadata

//    void FillMetadata(Type extensionType, object metadataInstance);

//    void FillMetadata(object extensionInstance, object metadataInstance);

//    #endregion

//    #region FillMetadata

//    void SatisfyImports(object targetObject);

//    #endregion
//  }
//}
