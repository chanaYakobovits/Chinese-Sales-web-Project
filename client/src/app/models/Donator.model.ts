import { Gift } from "./Gift.model";

export class Donator{
    id!: number;
    name!: string;
    email! : string;
    adress! : string;
    phone! : string;
    gifts!: Gift[];
}