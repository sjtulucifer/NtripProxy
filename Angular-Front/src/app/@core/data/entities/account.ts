import { Company } from "./company";
import { User } from "./user";

export class Account {
    constructor(
        public ID?: string,
        public Name?: string,
        public Password?: string,
        public Register?: Date,
        public Expire?: Date, 
        public LastLogin?: Date,
        public PasswordOvertime?: Date,
        public PasswordOvercount?: Number,
        public IsLocked?: boolean,
        public IsOnline?: boolean,
        public Company?: Company,
        public AddUser?: User,
        public IsDelete?: boolean
    ) { }
}
