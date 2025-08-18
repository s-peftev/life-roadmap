import { environment } from "../../environments/environment"

const basePath = environment.assetsBasePath;

function illustration(name: string) {
    return `${basePath}/img/illustrations/${name}`;
}

function validationIcon(name: string) {
    return `${basePath}/img/icons/validation/${name}`;
}

export const ASSETS = {
    IMAGES: {
        ILLUSTRATIONS: {
            TITLE: illustration('title.png'),
            REGISTER: illustration('register.png'),
            LOGIN: illustration('login.png')
        },
        ICONS: {
            VALIDATION: {
                REQUIRED: {
                    OK: validationIcon('required_ok.png'),
                    ERR: validationIcon('required_err.png'),
                },
                L_CASE: {
                    OK: validationIcon('lowercase_ok.png'),
                    ERR: validationIcon('lowercase_err.png'),
                },
                U_CASE: {
                    OK: validationIcon('uppercase_ok.png'),
                    ERR: validationIcon('uppercase_err.png'),
                },
                DIGIT: {
                    OK: validationIcon('digit_ok.png'),
                    ERR: validationIcon('digit_err.png'),
                },
                MIN: {
                    OK: validationIcon('min_ok.png'),
                    ERR: validationIcon('min_err.png'),
                },
                MAX: {
                    OK: validationIcon('max_ok.png'),
                    ERR: validationIcon('max_err.png'),
                },
                PASS_MATCH: {
                    OK: validationIcon('pass_match_ok.png'),
                    ERR: validationIcon('pass_match_err.png'),
                },
                EMAIL: {
                    OK: validationIcon('email_ok.png'),
                    ERR: validationIcon('email_err.png'),
                }
            }
        }
    }
}