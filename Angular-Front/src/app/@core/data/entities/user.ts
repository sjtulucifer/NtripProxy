import { Company } from "./company";
import { Role } from "./role";
import { Account } from "./account";

export class User {
    constructor(
        public ID?: string,
        public Login?: string,
        public Password?: string,
        public Name?: string,
        public Phone?: string,
        public Email?: string, 
        public Company?: Company,  
        public Accounts?: Array<Account>,     
        public Roles?: Array<Role>,
        public IsDelete?: boolean
    ) { }
}
