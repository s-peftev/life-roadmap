import { DefaultUrlSerializer, UrlTree, UrlSerializer } from '@angular/router';
import { Injectable } from '@angular/core';

@Injectable()
export class ModalOutletUrlSerializer implements UrlSerializer {
    private defaultSerializer = new DefaultUrlSerializer();

    parse(url: string): UrlTree {
        return this.defaultSerializer.parse(url);
    }

    serialize(tree: UrlTree): string {
        let url = this.defaultSerializer.serialize(tree);

        // instead of: /dashboard(modal:settings/profile)
        // does /settings/profile
        const match = url.match(/\(modal:([^)]*)\)$/);
        if (match) {
            url = '/' + match[1];
        }

        return url;
    }
}