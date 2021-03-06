import { Component, EventEmitter, Input, Output} from '@angular/core';
import { PortfolioItem } from '../models/portfolio-item';
import { TradingService } from '../services/trading.service';

@Component({
    selector: 'edit-item',
    templateUrl: './edit-item.component.html',
    styles: ['.modal { background: rgba(0,0,0,0.6); }']
})
export class EditItemComponent{
    @Input() item: PortfolioItem;
    @Output() afterSave: EventEmitter<any> = new EventEmitter();

    public visible = false;
    private visibleAnimate = false;

    constructor(private service: TradingService) {
    }

    public show(): void {
        this.visible = true;
        setTimeout(() => this.visibleAnimate = true);
    }

    public hide(): boolean {
        this.visibleAnimate = false;
        setTimeout(() => this.visible = false, 300);
        return false;
    }

    save() {
        this.service.savePortfolioItem(this.item).then(result => {
            this.hide();
            this.afterSave.emit();
        });
    }
}
