import { environment } from "../../environments/environment"

const basePath = environment.assetsBasePath;

function illustration(name: string) {
  return `${basePath}/img/illustrations/${name}`;
}

export const ASSETS = {
    IMAGES: {
        ILLUSTRATIONS : {
            TITLE: illustration('title.png'),
            REGISTER: illustration('register.png')
        }
    }
}