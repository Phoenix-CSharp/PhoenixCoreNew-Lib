namespace PhoenixCoreNew.FSharp

open System
open PhoenixCoreNew.FSharp.Logic
open PhoenixCoreNew.FSharp.Reflection
open PhoenixCoreNew.FSharp.Types

/// <summary>
/// Единая точка входа в F#-библиотеку (API Facade).
/// Предоставляет C#-приложению безопасные методы работы с атрибутами и битовыми флагами.
/// </summary>
type PhoenixCoreNew private () =

    /// <summary>
    /// Безопасная проверка наличия битового флага у любого Enum без Unsafe.As.
    /// </summary>
    static member HasFlag<'Enum when 'Enum : struct and 'Enum :> Enum>(mask: 'Enum, flag: 'Enum) : bool =
        BitmaskProcessor.hasFlag mask flag

    /// <summary>
    /// Извлечение активных битов из enum-маски в виде массива uint64 для C#.
    /// </summary>
    static member GetActiveFlags<'Enum when 'Enum : struct and 'Enum :> Enum>(mask: 'Enum) : uint64[] =
        BitmaskProcessor.extractActiveBits mask |> List.toArray

    /// <summary>
    /// Безопасное чтение атрибута с типа. Возвращает null для C#, если атрибут не найден.
    /// </summary>
    static member TryGetAttribute<'Attr when 'Attr :> Attribute>(targetType: Type) : 'Attr =
        match AttributeReader.tryGetAttribute<'Attr> targetType with
        | Some attr -> attr
        | None -> Unchecked.defaultof<'Attr>

    /// <summary>
    /// Проверяет, содержит ли Generic-атрибут (например Changer&lt;T&gt;) на указанном типе заданный Enum-флаг.
    /// </summary>
    static member CheckAttributeFlag<'Enum when 'Enum : struct and 'Enum :> Enum>
        (targetType: Type, genericAttributeName: string, flagToTest: 'Enum) : bool =
        
        match AttributeReader.findGenericAttribute targetType genericAttributeName with
        | Some attrInstance -> 
            AttributeReader.checkAttributeOption attrInstance "Options" flagToTest
        | None -> false