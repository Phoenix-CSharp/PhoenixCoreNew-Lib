namespace PhoenixCoreNew.FSharp.Logic

open System

module BitmaskProcessor =
    /// Приведение Enum к uint64 без использования Unsafe.As
    let inline enumToUInt64(value: 'Enum) : uint64
        when 'Enum :> Enum and 'Enum : struct =
        Convert.ToUInt64(value) |> uint64
    
    /// Универсальная проверка вхождения флага в битовую маску
    let inline hasFlag(mask : 'Enum ) (flag : 'Enum) : bool
        when 'Enum :> Enum and 'Enum : struct =
            let m : uint64 = enumToUInt64(mask)
            let f : uint64 = enumToUInt64(flag)
            (m &&& f) = m && f <> 0UL

    /// Разложение маски на список поднятых бит
    let extractActiveBits(mask : 'Enum) : uint64 list 
        when 'Enum :> Enum and 'Enum : struct =
        let m : uint64 = enumToUInt64 mask
        [ 0 .. 63 ]
        |> List.map (fun (i: int) -> 1UL <<< i)
        |> List.filter (fun (bit: uint64) -> (m &&& bit) = bit)