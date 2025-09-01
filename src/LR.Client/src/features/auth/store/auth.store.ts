import { signalStore, withComputed, withState } from "@ngrx/signals";
import { initialAuthSlice } from "./auth.slice";
import { computed } from "@angular/core";
import { withDevtools } from "@angular-architects/ngrx-toolkit";

export const AuthStore = signalStore(
    withState(initialAuthSlice),
    withComputed((store) => {
        const isAuthenticated = computed(() => 
            !!store.accessToken() && !!store.user());
        const hasValidAccessToken = computed(() => 
            !!store.accessToken() && !!store.user() && !!store.expiresAt() && store.expiresAt()! > new Date());

        return {
            isAuthenticated,
            hasValidAccessToken
        };
    }),
    withDevtools('auth-store')
);