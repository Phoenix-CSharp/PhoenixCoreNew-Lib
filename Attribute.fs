namespace PhoenixCoreNew.FSharp.Reflection

open System
open System.Reflection

module AttributeReader =

    /// Извлечение любого атрибута по его обобщенному типу
    let tryGetAttribute<'Attr when 'Attr :> Attribute> (targetType: Type) : 'Attr option =
        let attr: 'Attr = targetType.GetCustomAttribute<'Attr>()
        if Object.ReferenceEquals(attr, null) then None else Some attr

    /// Считывание значения публичного свойства/поля из любого экземпляра атрибута по имени
    let getPropertyValue(instance: obj)(propertyName: string): obj option =
        if Object.ReferenceEquals(instance, null) then None
        else 
            let prop: PropertyInfo = instance.GetType().GetProperty(propertyName, BindingFlags.Public ||| BindingFlags.Instance)
            if Object.ReferenceEquals(prop, null) then None
            else Some (prop.GetValue(instance))
    
    /// Поиск Generic-атрибута по имени класса атрибута
    let findGenericAttribute (targetType: Type) (genericAttributeTypeName: string) : obj option =
        targetType.GetCustomAttributes(true)
        |> Seq.tryFind (fun attr ->
            let t = attr.GetType()
            t.IsGenericType && t.Name.StartsWith(genericAttributeTypeName))

    /// Безопасная проверка: содержат ли параметры атрибута указанный enum-флаг
    let checkAttributeOption (attributeInstance: obj) (propertyName: string) (flagToTest: 'Enum) : bool 
        when 'Enum : struct and 'Enum :> Enum =
        match getPropertyValue attributeInstance propertyName with
        | Some (:? Enum as rawEnum) ->
            let maskValue = Convert.ToUInt64(rawEnum)
            let flagValue = Convert.ToUInt64(flagToTest)
            (maskValue &&& flagValue) = flagValue && flagValue <> 0UL
        | _ -> false
