import { environment } from "../../environments/environment"

const basePath = environment.assetsBasePath;

function illustration(name: string) {
    return `${basePath}/img/illustrations/${name}`;
}

function validationIcon(name: string) {
    return `${basePath}/img/icons/validation/${name}`;
}

function profileIcon(name: string) {
    return `${basePath}/img/icons/profile/${name}`;
}

function sidebarIcon(name: string) {
    return `${basePath}/img/icons/sidebar/${name}`;
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
            },
            PROFILE: {
                DEFAULT_AVATAR: profileIcon('default_avatar.png'),
                SETTINGS: profileIcon('settings.png'),
                LOGOUT: profileIcon('logout.png')
            },
            SIDEBAR: {
                SIDEBAR_TOGGLE: sidebarIcon('sidebar_toggle.png'),
                DASHBOARD: sidebarIcon('dashboard.png'),
                GOAL_WISHLIST: sidebarIcon('goal_wishlist.png'),
                STATISTICS: sidebarIcon('statistics.png'),
                ROADMAP: sidebarIcon('roadmap.png'),
                ADMIN: sidebarIcon('admin.png'),
            }
        }
    }
}