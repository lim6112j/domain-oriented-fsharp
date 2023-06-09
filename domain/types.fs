namespace OrderTracking
open System
module Domain =
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
  type Price = Price of int
  type UnvalidatedAddress = UnvalidatedAddress of string
  type ValidatedAddress = ValidatedAddress of string
  type UnitQuantity = private UnitQuantity of int
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
  type UnvalidatedCustomerInfo = Undefined
  type UnvalidatedOrder = {
    OrderId: string
    CustomerInfo: UnvalidatedCustomerInfo
    ShippingAddress : UnvalidatedAddress
  }
  type ValidatedOrderLine = Undefined
  type ValidatedOrder = {
    OrderId: OrderId
    CustomerInfo: CustomerInfo
    ShippingAddress: ValidatedAddress
    BillingAddress: ValidatedAddress
    OrderLines: ValidatedOrderLine list
  }
  type AcknowledgmentSent = Undefined
  type OrderPlaced = PricedOrder
  type BillableOrderPlaced = {
    OrderId : OrderId
    BillingAddress : ValidatedAddress
    AmountToBill : BillingAmount
  }
  type ContactId = ContactId of int
  type PhoneNumber = PhoneNumber of string
  type EmailAddress = EmailAddress of string
  type OrderAcknowledgmentSent = {
    OrderId : OrderId
    EmailAddress : EmailAddress
  }
  type PlaceOrderResult = {
    OrderPlaced : OrderPlaced
    BillableOrderPlaced : BillableOrderPlaced
    OrderAcknowledgmentSent : OrderAcknowledgmentSent option
  }
  type PlaceOrderEvent = 
    | OrderPlaced of OrderPlaced
    | BillableOrderPlaced of BillableOrderPlaced
    | AcknowledgmentSent of OrderAcknowledgmentSent
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
  type GetProductPrice =
    ProductCode -> Price
  type ValidationError = {
    FieldName : string
    ErrorDescription : string
  }
  type ValidationResponse<'a> = Async<Result<'a, ValidationError list>>
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
  type CheckAddressExists =
    UnvalidatedAddress -> Result<CheckedAddress, AddressValidationError>
  type ValidateOrder =
    CheckProductCodeExists
      -> CheckAddressExists
      -> UnvalidatedOrder
      -> Result<ValidatedOrder, ValidationError>
  //type PlaceOrder = {
  //  OrderForm: UnvalidatedOrder
  //  Timestamp: DateTime
  //  UserId: string
  //}
  type PlaceOrder = Command<UnvalidatedOrder>
  type ChangeOrder = Command<UnvalidatedOrder>
  type CancelOrder = Command<ValidatedOrder>
  type OrderTakingCommand =
    | Place of PlaceOrder
    | Change of ChangeOrder
    | Cancel of CancelOrder
  type PricedOrderLine = Undefined
  type PricedOrder = {
    OrderId: OrderId
    CustomerInfo: CustomerInfo
    ShippingAddress: ValidatedAddress
    OrderLines: PricedOrderLine list
    AmountToBill: BillingAmount
  }
  type PriceOrder = 
    GetProductPrice
      -> ValidateOrder
      -> PricedOrder
  type CalculatePrices = OrderForm -> ProductCatalog -> PriceOrder
  type Order = 
    | Unvalidated of UnvalidatedOrder
    | Validated of ValidatedOrder
    | Priced of PricedOrder
  type Item = Item of string
  type ActiveCartData = { UnpaidItems: Item list}
  type PaidCartData = { PaidItems: Item list; Payment: float }
  type ShoppingCart =
    | EmptyCart
    | ActiveCart of ActiveCartData
    | PaidCart of PaidCartData
  let addItem cart item =
    match cart with
    | EmptyCart ->
      ActiveCart {UnpaidItems=[item]}
    | ActiveCart { UnpaidItems=existingItems} ->
      ActiveCart {UnpaidItems = item :: existingItems}
    | PaidCart _ -> cart
  let makePayment cart payment =
    match cart with
    | EmptyCart -> cart
    | ActiveCart {UnpaidItems=existingItems} -> PaidCart {PaidItems = existingItems; Payment= payment}
    | PaidCart _ -> cart
  type HtmlString = HtmlString of string
  type OrderAcknowledgment = {
    EmailAddress : EmailAddress
    Letter : HtmlString
    }
  type CreateOrderAcknowledgmentLetter =
    PricedOrder -> HtmlString
  type SendResult = Sent | NotSent
  type SendOrderAcknowledgment =
    OrderAcknowledgment -> SendResult
  type AcknowledgmentOrder =
    CreateOrderAcknowledgmentLetter // dependency
      -> SendOrderAcknowledgment // dependency
      -> PricedOrder // input
      -> OrderAcknowledgmentSent option // output
  type CreateEvents = 
    PricedOrder -> PlaceOrderEvent list
