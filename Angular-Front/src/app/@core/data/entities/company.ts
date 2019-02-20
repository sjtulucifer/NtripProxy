import { Account } from "./account";
import { User } from "./user";

export class Company {
    constructor(
        public ID?: string,
        public Name?: string,
        public Corporation?: string,
        public Qualification?: string,
        public QNo?: string, 
        public Field?: string,
        public Contract?: string,
        public Phone?: string,
        public Address?: string,
        public Accounts?: Array<Account>,
        public SubCompanies?: Array<Company>,
        public ParentCompany?: Company,
        public Users?: Array<User>,
        public IsDelete?: boolean
    ) { }
}
