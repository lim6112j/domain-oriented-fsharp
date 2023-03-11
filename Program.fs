namespace OrderTracking// For more information see https://aka.ms/fsharp-console-apps
open OrderTracking.Domain
module Program =
  printfn "hello world"
  let customerId = CustomerId 42
  let orderId = OrderId 42
  let (CustomerId innervalue) = customerId
  printfn "customer id value : %i" innervalue
  let processCustomerId (CustomerId innerV) =
    printfn "innervalue is %i" innerV
  processCustomerId(customerId)
  //printfn "customer id = order id : %b" (customerId = orderId)
  let contactId = ContactId 1
  let contact1 = 
    {
      ContactId=contactId
      PhoneNumber = PhoneNumber "111-111-1111"
      EmailAddress = EmailAddress "sd@hkjl.com"
    }
  let contact2 = 
    {
      ContactId=contactId
      PhoneNumber = PhoneNumber "111-111-1111"
      EmailAddress = EmailAddress "sd@hkjl.com"
    }
  printfn "compaing 2 contact : %b" (contact1 = contact2)
  let initialContact = {ContactId=ContactId 42; PhoneNumber=PhoneNumber "111-0111-111"; EmailAddress=EmailAddress "ddd@ddd.com"}
  let updatedContact = {initialContact with ContactId = ContactId 41}
  printfn "initial : %A,\nupdated: %A" initialContact updatedContact
