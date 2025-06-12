import { User } from "./User.model";

export class Purchase{  
    name!: string;
    customers!: User[];
    count!: number;
}