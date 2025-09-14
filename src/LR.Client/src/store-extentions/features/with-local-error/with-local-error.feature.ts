import { signalStoreFeature, SignalStoreFeature, withState } from "@ngrx/signals"
import { Signal } from "@angular/core"
import { initialLocalErrorSlice, LocalErrorSlice } from "./with-local-error.slice"

export function withLocalError(): SignalStoreFeature<{
    state: {}, 
    props: {}, 
    methods: {}
}, {
    state: LocalErrorSlice, 
    props: {}, 
    methods: {}
}>;

export function withLocalError(): SignalStoreFeature {
    return signalStoreFeature(
        withState(initialLocalErrorSlice), 
    )
}