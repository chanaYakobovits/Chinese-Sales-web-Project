export class Order{
    id!:number 
    customerId:number=0
    giftId!:number
    dateOrder :Date = new Date()
    isDraft:boolean=false
}