namespace OrderTracking.Domain

type Result<'Success, 'Failure> =
  | OK  of 'Success
  | Error of 'Failure

type WidgetCode = WidgetCode of string
type GizmoCode = GizmoCode of string
type ProductCode =
  | Widget of WidgetCode 
  | Gizmo of GizmoCode
type UnitQuantity = private UnitQuantity of int
type KilogramQuantity = KilogramQuantity of decimal
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
    OrderLines : OrderLine list
    AmountToBill : BillingAmount
}
type UnvalidatedOrder = Undefined
type ValidatedOrder = Undefined
type AcknowledgmentSent = Undefined
type OrderPlaced = Undefined
type BillableOrderPlaced = Undefined
type PlaceOrderEvents = {
  AcknowledgmentSent : AcknowledgmentSent
  OrderPlaced : OrderPlaced
  BillableOrderPlaced : BillableOrderPlaced
}
type PlaceOrder = UnvalidatedOrder -> PlaceOrderEvents
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
type ValidateOrder =
  UnvalidatedOrder -> ValidationResponse<ValidatedOrder>
type ContactId = ContactId of int
type PhoneNumber = PhoneNumber of string
type EmailAddress = EmailAddress of string

[<CustomEquality; NoComparison>]
type Contact = {
  ContactId : ContactId
  PhoneNumber : PhoneNumber
  EmailAddress : EmailAddress
} with
override this.Equals(obj) =
  match obj with
  | :? Contact as c -> this.ContactId = c.ContactId
  | _ -> false
override this.GetHashCode() =
  hash this.ContactId
end
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
