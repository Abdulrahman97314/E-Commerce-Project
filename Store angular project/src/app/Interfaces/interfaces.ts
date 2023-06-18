export interface Product {
  id: number;
  name: string;
  description: string;
  pictureUrl: string;
  price: number;
  productBrand: string;
  productBrandId?: number;
  productType: string;
  productTypeId: number;
  productRating: ProductRating[];
  averageRating: number;
}
export interface BrandOrType {
  name: string;
  id: number;
}
export interface ProductRating {
  userId: string;
  productId: number;
  rating: number;
  message: string;
  userName: string;
  dateTime: string
}

export interface PaginatedResult {
  pageIndex: number;
  pageSize: number;
  count: number;
  data: Product[];
}
export interface RegisterForm {
  displayName: string;
  email: string;
  phoneNumber: number;
  password: string;
  confirmPassword: string;
}
export interface LoginForm {
  email: String;
  password: string;
}
export interface UserData {
  displayName: string,
  email: string,
  token: string
}
export interface DecodedData {
  aud: string,
  email: string,
  exp: number,
  given_name: string,
  iat: number,
  iss: string,
  nbf: number,
  role: String
}
export interface AddRating {
  productId: number,
  rating: number,
  message: string
}
export interface ForgetPassword {
  email: string
}
export interface ResetPassword {
  email: string,
  newPassword: string,
  confirmPassword: string,
  token: string
}
export interface ApiResponse{
  statusCode:number,
  message:string
}
export interface BasketItem {
  id: number;
  productName: string;
  pictureUrl: string;
  price: number;
  brand: string;
  type: string;
  quantity: number;
}
export interface Basket{
    id: string,
    items:BasketItem [],
    paymentIntentId?: string,
    clientSecret?: string,
    deliveryMethodId?: number,
    shippingPrice?: number
}
export interface DeliveryMethod {
  shortName: string,
  description: string,
  deliveryTime: string,
  cost: number,
  id: number
}
export interface Order {
  basketId: string,
  deliveryMethodId:number,
  shippingAddress:ShippingAddress
}
export interface OrderReturn {
    id: number,
    buyerEmail: string,
    orderDate: string,
    status: string,
    shippingAddress:ShippingAddress
    deliveryMethod: DeliveryMethod
    items: BasketItem[]
    subTotal: number,
    total?:number,
    paymentIntentId: string
}
export interface ShippingAddress{
  firstName: string,
  lastName: string,
  street: string,
  city: string,
  country: string
}
