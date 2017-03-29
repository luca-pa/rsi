import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { UniversalModule } from 'angular2-universal';
import { FormsModule } from '@angular/forms';
import { ModalModule } from 'angular2-modal';
import { BootstrapModalModule } from 'angular2-modal/plugins/bootstrap';
import { AppComponent } from './components/app.component'
import { NavMenuComponent } from './components/navmenu.component';
import { HomeComponent } from './components/home.component';
import { PortfolioComponent } from './components/portfolio.component';
import { EditItemComponent } from './components/edit-item.component';
import { NewItemComponent } from './components/new-item.component';
import { EditBilancioComponent } from './components/edit-bilancio.component';
import { RankingComponent } from './components/ranking.component';
import { AllRankingComponent } from './components/allranking.component';
import { ChartsComponent } from './components/charts.component';
import { TradingService } from './services/trading.service';
import { DateService } from './services/date.service';

@NgModule({
    bootstrap: [ AppComponent ],
    declarations: [
        AppComponent,
        NavMenuComponent,
        EditItemComponent,
        NewItemComponent,
        EditBilancioComponent,
        PortfolioComponent,
        RankingComponent,
        AllRankingComponent,
        ChartsComponent,
        HomeComponent
    ],
    providers: [ TradingService, DateService ],
    imports: [
        UniversalModule, // Must be first import. This automatically imports BrowserModule, HttpModule, and JsonpModule too.
        FormsModule,
        ModalModule,
        BootstrapModalModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'portfolio', component: PortfolioComponent },
            { path: 'ranking', component: RankingComponent },
            { path: 'all', component: AllRankingComponent },
            { path: 'charts', component: ChartsComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ]
})
export class AppModule {
}
