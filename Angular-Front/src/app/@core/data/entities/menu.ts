import { Permission } from "./permission";

export class Menu {
    constructor(
        public ID?: string,
        public Catagory?: string,
        public Name?: string,
        public Url?: string,
        public Description?: string, 
        public Permissions?: Array<Permission>,
        public IsDelete?: boolean
    ) { }
}
