import { Role } from "./role";
import { Menu } from "./menu";

export class Permission {
    constructor(
        public ID?: string,
        public Name?: string,
        public Description?: string, 
        public Roles?: Array<Role>,     
        public Menus?: Array<Menu>,
        public IsDelete?: boolean
    ) { }
}
