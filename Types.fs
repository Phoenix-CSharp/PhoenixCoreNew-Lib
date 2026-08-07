namespace PhoenixCoreNew.FSharp.Types

open System

/// Перечень всех ошибок, которые необходимо отлавливать
type LibError=
    | AttributeNotFound of targetType: Type * attributeName : string
    | PropetryNotFound of targetType : Type * propertyName: string
    | FlagMismatch of flagValue: uint64 * expectedInterface : string
    | InvalidDataRange of message: string

/// Результат выполнения операций
type Result<'T> = Result<'T, LibError list>