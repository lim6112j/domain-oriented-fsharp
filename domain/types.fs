namespace OrderTracking.Domain
open System
[<Measure>]
type kg
[<Measure>]
type m
type Result<'Success, 'Failure> =
  | OK  of 'Success
  | Error of 'Failure
type Command<'data> = {
  Data: 'data
  Timestamp: DateTime
  UserId: string
}
type UnvalidatedAddress = UnvalidatedAddress of string
type ValidatedAddress = ValidatedAddress of string
type UnitQuantity = private UnitQuantity of int
module UnitQuantity =
  let create qty : Result<UnitQuantity, string>=
    if qty < 1 then
      Error "UnitQuantity can not be negative"
    else if qty > 1000 then
      Error "UnitQuantity can not be more than 1000"
    else 
      OK (UnitQuantity qty)
  // return wrapped value
  let value (UnitQuantity qty) = qty
type NonEmptyList<'a> = {
  First: 'a
  Rest: 'a list
}
type WidgetCode = WidgetCode of string
type GizmoCode = GizmoCode of string
type ProductCode =
  | Widget of WidgetCode 
  | Gizmo of GizmoCode
type KilogramQuantity = KilogramQuantity of decimal<kg>
type OrderQuantity =
  | Unit of UnitQuantity
  | Kilogram of KilogramQuantity

type CustomerId = CustomerId of int
type OrderId = OrderId of int
type Undefined = exn
type CustomerInfo = Undefined
type ShippingAddress = Undefined
type BillingAddress = Undefined
type OrderLine = Undefined
type BillingAmount = Undefined
type Order = {
    CustomerInfo : CustomerInfo
    ShippingAddress : ShippingAddress
    BillingAddress : BillingAddress
    OrderLines : NonEmptyList<OrderLine> // non empty list
    AmountToBill : BillingAmount
}
type UnvalidatedCustomerInfo = Undefined
type UnvalidatedOrder = {
  OrderId: string
  CustomerInfo: UnvalidatedCustomerInfo
  ShippingAddress : UnvalidatedAddress
}
type ValidatedOrder = {
  ShippingAddress: ValidatedAddress
}
type AcknowledgmentSent = Undefined
type OrderPlaced = Undefined
type BillableOrderPlaced = Undefined
type PlaceOrderEvents = {
  AcknowledgmentSent : AcknowledgmentSent
  OrderPlaced : OrderPlaced
  BillableOrderPlaced : BillableOrderPlaced
}
type QuoteForm = Undefined
type OrderForm = Undefined
type CategorizedMail =
  | Quote of QuoteForm
  | Order of OrderForm

  type EnvelopeContents = Undefined
type CategorizedInboundMail = EnvelopeContents -> CategorizedMail
type ProductCatalog = Undefined
type PriceOrder = Undefined
type CalculatePrices = OrderForm -> ProductCatalog -> PriceOrder
type ValidationError = {
  FieldName : string
  ErrorDescription : string
}
type ValidationResponse<'a> = Async<Result<'a, ValidationError list>>
type ContactId = ContactId of int
type PhoneNumber = PhoneNumber of string
type EmailAddress = EmailAddress of string
type BothContactMethods = {
  email: EmailAddress
  phone: PhoneNumber
}
// business rule, email only, phone only, or both
type ContactInfo =
  | EmailOnly of EmailAddress
  | PhoneOnly of PhoneNumber
  | EmailAndAddr of BothContactMethods
[<CustomEquality; NoComparison>]
type Contact = {
  ContactId : ContactId
  ContactInfo: ContactInfo
} with
override this.Equals(obj) =
  match obj with
  | :? Contact as c -> this.ContactId = c.ContactId
  | _ -> false
override this.GetHashCode() =
  hash this.ContactId
end
type CheckProductCodeExists =
  ProductCode -> bool
type CheckedAddress = CheckedAddress of UnvalidatedAddress
type AddressValidationError = AddressValidationError of string
type CheckedAddressExists =
  UnvalidatedAddress -> Result<CheckedAddress, AddressValidationError>
type ValidateOrder =
  CheckProductCodeExists
    -> CheckedAddressExists
    -> UnvalidatedOrder
    -> Result<ValidatedOrder, ValidationError>

//type PlaceOrder = {
//  OrderForm: UnvalidatedOrder
//  Timestamp: DateTime
//  UserId: string
//}
type PlaceOrder = Command<UnvalidatedOrder>
