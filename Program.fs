namespace DomainTypes// For more information see https://aka.ms/fsharp-console-apps
open BusinessLogic
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
