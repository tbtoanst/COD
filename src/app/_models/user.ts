export interface User {
    id: number;
    username: string;
    fullname: string;
    email: string;
    role: any;
    positions: {
        branch: {
            id: string,
            code: string,
            name: string,
            address: string
        };
        department: {
            id: string,
            code: string,
            level: number,
            parent_id: string,
            name: string,
            description: string,
            short_name: string
        }
    };    
}