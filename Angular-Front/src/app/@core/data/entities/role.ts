import { User } from "./user";
import { Permission } from "./permission";

export class Role {
    constructor(
        public ID?: string,
        public Name?: string,
        public Description?: string, 
        public Users?: Array<User>,     
        public Permissions?: Array<Permission>,
        public IsDelete?: boolean
    ) { }
}
