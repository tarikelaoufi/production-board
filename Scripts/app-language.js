/*
 * app-language.js
 * Shared EN / FR language selector for the complete Web Forms application.
 *
 * Usage:
 *     <script src="Scripts/app-language.js"></script>
 *
 * The selected language is stored in:
 * - localStorage: productionBoardLanguage
 * - cookie: productionBoardLanguage
 *
 * Add data-i18n="translation.key" to any element when a precise translation
 * is required. The file also translates the common text used by the current
 * application pages automatically.
 */
(function (window, document) {
    "use strict";

    var STORAGE_KEY = "productionBoardLanguage";
    var COOKIE_KEY = "productionBoardLanguage";
    var supportedLanguages = ["en", "fr"];
    var currentLanguage = readStoredLanguage();
    var isApplyingTranslations = false;
    var mutationObserver = null;

    var keyedTranslations = {
        en: {
            "nav.productionBoard": "Production Board",
            "nav.boardInfo": "Board information",
            "nav.previousTeam": "Previous team",
            "nav.nextTeam": "Next team",
            "nav.weekly": "Weekly Synthesis",
            "nav.findBoard": "Find a board",
            "nav.changeBoard": "Change board",
            "nav.signOut": "Sign out",
            "nav.language": "Language",
            "page.title": "Production Board",
            "page.subtitle": "Use the animated arrows in the navigation bar to move between team boards.",
            "page.boardTitle": "Production Board",
            "info.date": "Date",
            "info.shift": "Shift",
            "info.team": "Team",
            "info.product": "Product",
            "info.line": "Line",
            "table.target": "Target",
            "table.cumulative": "Cumulative",
            "table.actual": "Actual",
            "table.scrap": "Scrap",
            "table.comments": "Comments",
            "table.update": "Update",
            "timeline.title": "Shift timeline",
            "timeline.pending": "Pending",
            "timeline.actual": "Actual",
            "timeline.scrap": "Scrap",
            "boardInfo.title": "Production board information",
            "boardInfo.subtitle": "Current production board information.",
            "boardInfo.id": "Board ID",
            "boardInfo.productionLine": "Production line",
            "boardInfo.accessMode": "Access mode",
            "boardInfo.editable": "Editable board",
            "boardInfo.readOnly": "Read-only board",
            "modal.title": "Update production hour",
            "modal.subtitle": "Move between H1 and H8 without closing the modal.",
            "modal.actualQuantity": "Actual quantity",
            "modal.scrapQuantity": "Scrap quantity",
            "modal.commentPlaceholder": "Describe a stop, incident or observation...",
            "common.close": "Close",
            "common.save": "Save",
            "common.saveClose": "Save & Close"
        },
        fr: {
            "nav.productionBoard": "Tableau de marche",
            "nav.boardInfo": "Informations du tableau",
            "nav.previousTeam": "\u00C9quipe pr\u00E9c\u00E9dente",
            "nav.nextTeam": "\u00C9quipe suivante",
            "nav.weekly": "Synth\u00E8se hebdomadaire",
            "nav.findBoard": "Rechercher un tableau",
            "nav.changeBoard": "Changer de tableau",
            "nav.signOut": "D\u00E9connexion",
            "nav.language": "Langue",
            "page.title": "Tableau de marche",
            "page.subtitle": "Utilisez les fl\u00E8ches anim\u00E9es pour passer entre les tableaux des \u00E9quipes.",
            "page.boardTitle": "Tableau de marche",
            "info.date": "Date",
            "info.shift": "Poste",
            "info.team": "\u00C9quipe",
            "info.product": "Produit",
            "info.line": "Ligne",
            "table.target": "Objectif",
            "table.cumulative": "Cumul",
            "table.actual": "R\u00E9el",
            "table.scrap": "Rebut",
            "table.comments": "Commentaires",
            "table.update": "Modifier",
            "timeline.title": "Chronologie du poste",
            "timeline.pending": "En attente",
            "timeline.actual": "R\u00E9el",
            "timeline.scrap": "Rebut",
            "boardInfo.title": "Informations du tableau de marche",
            "boardInfo.subtitle": "Informations du tableau de production actuel.",
            "boardInfo.id": "ID du tableau",
            "boardInfo.productionLine": "Ligne de production",
            "boardInfo.accessMode": "Mode d\u2019acc\u00E8s",
            "boardInfo.editable": "Tableau modifiable",
            "boardInfo.readOnly": "Tableau en lecture seule",
            "modal.title": "Mise \u00E0 jour de l\u2019heure de production",
            "modal.subtitle": "Passez de H1 \u00E0 H8 sans fermer la fen\u00EAtre.",
            "modal.actualQuantity": "Quantit\u00E9 r\u00E9elle",
            "modal.scrapQuantity": "Quantit\u00E9 rebut",
            "modal.commentPlaceholder": "D\u00E9crivez un arr\u00EAt, un incident ou une observation...",
            "common.close": "Fermer",
            "common.save": "Enregistrer",
            "common.saveClose": "Enregistrer et fermer"
        }
    };

    /*
     * Exact common UI texts.
     * This allows existing pages to use the shared language system without
     * rewriting every ASPX element immediately.
     */
    var exactTranslations = {
        en: {
            "Tableau de marche": "Production Board",
            "Informations du tableau": "Board information",
            "\u00C9quipe pr\u00E9c\u00E9dente": "Previous team",
            "\u00C9quipe suivante": "Next team",
            "Synth\u00E8se hebdomadaire": "Weekly Synthesis",
            "Rechercher un tableau": "Find a board",
            "Changer de tableau": "Change board",
            "D\u00E9connexion": "Sign out",
            "Langue": "Language",
            "Ouvrir le tableau de marche": "Open Production Board",
            "S\u00E9lectionnez le contexte de production avant de continuer.": "Select the production context before continuing.",
            "Utilisateur connect\u00E9": "Signed-in user",
            "R\u00F4le": "Role",
            "\u00C9quipe affect\u00E9e": "Assigned team",
            "\u00C9quipe": "Team",
            "Poste": "Shift",
            "Ligne de production": "Production line",
            "Produit": "Product",
            "Date du tableau": "Board date",
            "Continuer vers le tableau de marche": "Continue to Production Board",
            "Rechercher un tableau de production": "Find a Production Board",
            "Recherchez les tableaux enregistr\u00E9s et ouvrez le tableau horaire complet.": "Search saved boards and open the complete hourly table.",
            "Ouvrir ou cr\u00E9er un tableau": "Open or create board",
            "\u00C9quipe autoris\u00E9e": "Authorized team",
            "Toutes les \u00E9quipes": "All teams",
            "Tous les postes": "All shifts",
            "Toutes les lignes de production": "All production lines",
            "Tous les produits": "All products",
            "Effacer": "Clear",
            "Rechercher les tableaux": "Search boards",
            "Tableaux enregistr\u00E9s": "Saved boards",
            "Aucun tableau de production ne correspond aux filtres s\u00E9lectionn\u00E9s.": "No production board matches the selected filters.",
            "Derni\u00E8re mise \u00E0 jour": "Last update",
            "Ouvrir le tableau": "Open board",
            "Semaine contenant": "Week containing",
            "Production, objectif, rebut et efficacit\u00E9 par jour et par \u00E9quipe.": "Production, target, scrap and efficiency by day and team.",
            "Meilleure \u00E9quipe": "Best team",
            "Objectif hebdomadaire": "Weekly target",
            "Production hebdomadaire": "Weekly production",
            "Rebut hebdomadaire": "Weekly scrap",
            "Efficacit\u00E9": "Efficiency",
            "Objectif quotidien et r\u00E9el": "Daily target vs actual",
            "R\u00E9el compar\u00E9 \u00E0 l\u2019objectif": "Actual compared with target",
            "Production par \u00E9quipe": "Production by team",
            "Production r\u00E9elle la plus \u00E9lev\u00E9e": "Highest actual production",
            "R\u00E9partition des rebuts": "Scrap distribution",
            "D\u00E9tails hebdomadaires": "Weekly details",
            "Jour": "Day",
            "Objectif": "Target",
            "R\u00E9el": "Actual",
            "Rebut": "Scrap",
            "Fermer": "Close",
            "Enregistrer": "Save",
            "Enregistrer et fermer": "Save & Close",
            "Choisir la langue": "Choose language",
            "S\u00E9lectionnez la langue utilis\u00E9e dans toute l\u2019application.": "Select the language used across the application.",
            "Anglais": "English",
            "Fran\u00E7ais": "French",
            "Appliquer les filtres": "Apply filters",
            "Aucune donn\u00E9e de production n\u2019est disponible pour la semaine et les filtres s\u00E9lectionn\u00E9s.": "No production data is available for the selected week and filters.",
            "Les filtres de recherche ont \u00E9t\u00E9 effac\u00E9s.": "The search filters were cleared.",
            "Aucun tableau disponible": "No board available",
            "Erreur de recherche": "Search error",
            "Recherche termin\u00E9e": "Search completed",
            "Informations": "Information",
            "Filtres effac\u00E9s": "Filters cleared",
            "Recherche invalide": "Invalid search",
            "Aucun tableau de production n\u2019est disponible pour les informations s\u00E9lectionn\u00E9es.": "There is no available production board for the selected information.",
            "S\u00E9lectionnez une date de tableau valide.": "Select a valid board date.",
            "S\u00E9lectionnez une \u00E9quipe valide.": "Select a valid team.",
            "S\u00E9lectionnez un poste valide.": "Select a valid shift.",
            "S\u00E9lectionnez une ligne de production.": "Select a production line.",
            "S\u00E9lectionnez un produit.": "Select a product.",
            "S\u00E9lectionnez une date valide pour le tableau de production.": "Select a valid production-board date.",
            "Aucune affectation d\u2019\u00E9quipe active n\u2019existe pour ce compte.": "No active team assignment exists for this account."
        },
        fr: {
            "Production Board": "Tableau de marche",
            "Board information": "Informations du tableau",
            "Previous team": "\u00C9quipe pr\u00E9c\u00E9dente",
            "Next team": "\u00C9quipe suivante",
            "Weekly Synthesis": "Synth\u00E8se hebdomadaire",
            "Weekly synthesis": "Synth\u00E8se hebdomadaire",
            "Find a board": "Rechercher un tableau",
            "Find board": "Rechercher un tableau",
            "Change board": "Changer de tableau",
            "Sign out": "D\u00E9connexion",
            "Language": "Langue",
            "Open Production Board": "Ouvrir le tableau de marche",
            "Select the production context before continuing.": "S\u00E9lectionnez le contexte de production avant de continuer.",
            "Signed-in user": "Utilisateur connect\u00E9",
            "Role": "R\u00F4le",
            "Assigned team": "\u00C9quipe affect\u00E9e",
            "Authorized team": "\u00C9quipe autoris\u00E9e",
            "Team": "\u00C9quipe",
            "Shift": "Poste",
            "Production line": "Ligne de production",
            "Product": "Produit",
            "Board date": "Date du tableau",
            "Continue to Production Board": "Continuer vers le tableau de marche",
            "Viewer access is read-only. Production modifications are not permitted.": "L\u2019acc\u00E8s Viewer est en lecture seule. Les modifications de production ne sont pas autoris\u00E9es.",
            "Find a Production Board": "Rechercher un tableau de production",
            "Search saved boards and open the complete hourly table.": "Recherchez les tableaux enregistr\u00E9s et ouvrez le tableau horaire complet.",
            "Open or create board": "Ouvrir ou cr\u00E9er un tableau",
            "All teams": "Toutes les \u00E9quipes",
            "All shifts": "Tous les postes",
            "All production lines": "Toutes les lignes de production",
            "All products": "Tous les produits",
            "Clear": "Effacer",
            "Search boards": "Rechercher les tableaux",
            "Saved boards": "Tableaux enregistr\u00E9s",
            "No production board matches the selected filters.": "Aucun tableau de production ne correspond aux filtres s\u00E9lectionn\u00E9s.",
            "Date": "Date",
            "Line": "Ligne",
            "Last update": "Derni\u00E8re mise \u00E0 jour",
            "Open board": "Ouvrir le tableau",
            "Week containing": "Semaine contenant",
            "Production, target, scrap and efficiency by day and team.": "Production, objectif, rebut et efficacit\u00E9 par jour et par \u00E9quipe.",
            "Best team": "Meilleure \u00E9quipe",
            "Weekly target": "Objectif hebdomadaire",
            "Weekly production": "Production hebdomadaire",
            "Weekly scrap": "Rebut hebdomadaire",
            "Efficiency": "Efficacit\u00E9",
            "Daily target vs actual": "Objectif quotidien et r\u00E9el",
            "Actual compared with target": "R\u00E9el compar\u00E9 \u00E0 l\u2019objectif",
            "Production by team": "Production par \u00E9quipe",
            "Highest actual production": "Production r\u00E9elle la plus \u00E9lev\u00E9e",
            "Scrap distribution": "R\u00E9partition des rebuts",
            "Weekly details": "D\u00E9tails hebdomadaires",
            "Day": "Jour",
            "Target": "Objectif",
            "Actual": "R\u00E9el",
            "Scrap": "Rebut",
            "Comments": "Commentaires",
            "Cumulative": "Cumul",
            "Update": "Modifier",
            "Close": "Fermer",
            "Save": "Enregistrer",
            "Save & Close": "Enregistrer et fermer",
            "Choose language": "Choisir la langue",
            "Select the language used across the application.": "S\u00E9lectionnez la langue utilis\u00E9e dans toute l\u2019application.",
            "English": "Anglais",
            "French": "Fran\u00E7ais",
            "Apply filters": "Appliquer les filtres",
            "No production data is available for the selected week and filters.": "Aucune donn\u00E9e de production n\u2019est disponible pour la semaine et les filtres s\u00E9lectionn\u00E9s.",
            "The search filters were cleared.": "Les filtres de recherche ont \u00E9t\u00E9 effac\u00E9s.",
            "No board available": "Aucun tableau disponible",
            "Search error": "Erreur de recherche",
            "Search completed": "Recherche termin\u00E9e",
            "Information": "Informations",
            "Filters cleared": "Filtres effac\u00E9s",
            "Invalid search": "Recherche invalide",
            "There is no available production board for the selected information.": "Aucun tableau de production n\u2019est disponible pour les informations s\u00E9lectionn\u00E9es.",
            "Select a valid board date.": "S\u00E9lectionnez une date de tableau valide.",
            "Select a valid team.": "S\u00E9lectionnez une \u00E9quipe valide.",
            "Select a valid shift.": "S\u00E9lectionnez un poste valide.",
            "Select a production line.": "S\u00E9lectionnez une ligne de production.",
            "Select a product.": "S\u00E9lectionnez un produit.",
            "Select a valid production-board date.": "S\u00E9lectionnez une date valide pour le tableau de production.",
            "No active team assignment exists for this account.": "Aucune affectation d\u2019\u00E9quipe active n\u2019existe pour ce compte.",
            "The saved boards could not be loaded. Check the SQL Server connection and try again.": "Les tableaux enregistr\u00E9s n\u2019ont pas pu \u00EAtre charg\u00E9s. V\u00E9rifiez la connexion SQL Server et r\u00E9essayez.",
            "Viewer access: you may open boards from every team and shift, but the production table is read-only.": "Acc\u00E8s Viewer : vous pouvez ouvrir les tableaux de toutes les \u00E9quipes et de tous les postes, mais le tableau de production est en lecture seule.",
            "Administrator access: you may search and open boards from every team and shift.": "Acc\u00E8s administrateur : vous pouvez rechercher et ouvrir les tableaux de toutes les \u00E9quipes et de tous les postes."
        }
    };

    var titleTranslations = {
        en: {
            "Tableau de marche": "Production Board",
            "Synth\u00E8se hebdomadaire": "Weekly Synthesis",
            "Rechercher un tableau de production": "Find Production Board",
            "Tableau de marche - Configuration": "Production Board - Board Setup"
        },
        fr: {
            "Production Board": "Tableau de marche",
            "Weekly Synthesis": "Synth\u00E8se hebdomadaire",
            "Find Production Board": "Rechercher un tableau de production",
            "Production Board - Board Setup": "Tableau de marche - Configuration"
        }
    };

    function normalizeText(value) {
        return String(value || "")
            .replace(/\s+/g, " ")
            .trim();
    }

    function readStoredLanguage() {
        var storedLanguage = null;

        try {
            storedLanguage = window.localStorage.getItem(STORAGE_KEY);
        } catch (storageError) {
            storedLanguage = null;
        }

        if (supportedLanguages.indexOf(storedLanguage) >= 0) {
            return storedLanguage;
        }

        var cookieValue = readCookie(COOKIE_KEY);

        return supportedLanguages.indexOf(cookieValue) >= 0
            ? cookieValue
            : "en";
    }

    function readCookie(name) {
        var encodedName = encodeURIComponent(name) + "=";
        var cookieParts = String(document.cookie || "").split(";");

        for (var index = 0; index < cookieParts.length; index++) {
            var part = cookieParts[index].trim();

            if (part.indexOf(encodedName) === 0) {
                return decodeURIComponent(
                    part.substring(encodedName.length)
                );
            }
        }

        return null;
    }

    function persistLanguage(language) {
        try {
            window.localStorage.setItem(
                STORAGE_KEY,
                language
            );
        } catch (storageError) {
            /* Cookie storage below is still available. */
        }

        document.cookie =
            encodeURIComponent(COOKIE_KEY) +
            "=" +
            encodeURIComponent(language) +
            "; path=/; max-age=31536000; SameSite=Lax";
    }

    function getKeyedTranslation(key) {
        var languageDictionary =
            keyedTranslations[currentLanguage] ||
            keyedTranslations.en;

        return languageDictionary[key] ||
            keyedTranslations.en[key] ||
            key;
    }

    function getExactTranslation(text) {
        var cleanText = normalizeText(text);

        if (!cleanText) {
            return null;
        }

        var languageDictionary =
            exactTranslations[currentLanguage] || {};

        if (Object.prototype.hasOwnProperty.call(
                languageDictionary,
                cleanText)) {
            return languageDictionary[cleanText];
        }

        /*
         * Dynamic Find Board toast:
         * "3 production boards were found."
         */
        var resultMatch = cleanText.match(
            /^(\d+)\s+production board(?:s)?\s+(?:was|were)\s+found\.$/i
        );

        if (resultMatch && currentLanguage === "fr") {
            var count = Number(resultMatch[1]);

            return count +
                (count === 1
                    ? " tableau de production trouv\u00E9."
                    : " tableaux de production trouv\u00E9s.");
        }

        /*
         * Dynamic count label:
         * "1 board" / "5 boards"
         */
        var boardCountMatch = cleanText.match(
            /^(\d+)\s+board(?:s)?$/i
        );

        if (boardCountMatch && currentLanguage === "fr") {
            var boardCount = Number(boardCountMatch[1]);

            return boardCount +
                (boardCount === 1
                    ? " tableau"
                    : " tableaux");
        }

        var teamLeaderAccessMatch =
            cleanText.match(
                /^Team-leader access: search is restricted to (.+)\. You may search every shift for this team\.$/i
            );

        if (teamLeaderAccessMatch &&
            currentLanguage === "fr") {
            return "Acc\u00E8s chef d\u2019\u00E9quipe : la recherche est limit\u00E9e \u00E0 " +
                teamLeaderAccessMatch[1] +
                ". Vous pouvez rechercher tous les postes de cette \u00E9quipe.";
        }

        return null;
    }

    function replaceTextNode(node) {
        if (!node || node.nodeType !== 3) {
            return;
        }

        var parent = node.parentElement;

        if (!parent ||
            parent.closest("[data-app-language-ignore]") ||
            /^(SCRIPT|STYLE|TEXTAREA|CODE|PRE)$/i.test(parent.tagName)) {
            return;
        }

        var original = node.nodeValue;
        var cleanText = normalizeText(original);
        var translated = getExactTranslation(cleanText);

        if (!translated || translated === cleanText) {
            return;
        }

        var leadingWhitespace =
            original.match(/^\s*/)[0];

        var trailingWhitespace =
            original.match(/\s*$/)[0];

        node.nodeValue =
            leadingWhitespace +
            translated +
            trailingWhitespace;
    }

    function translateElementByKey(element) {
        var key = element.getAttribute("data-i18n");

        if (!key) {
            return;
        }

        element.textContent =
            getKeyedTranslation(key);
    }

    function translateElementAttributes(element) {
        var placeholderKey =
            element.getAttribute("data-i18n-placeholder");

        if (placeholderKey) {
            element.setAttribute(
                "placeholder",
                getKeyedTranslation(placeholderKey)
            );
        } else if (element.hasAttribute("placeholder")) {
            var translatedPlaceholder =
                getExactTranslation(
                    element.getAttribute("placeholder")
                );

            if (translatedPlaceholder) {
                element.setAttribute(
                    "placeholder",
                    translatedPlaceholder
                );
            }
        }

        var titleKey =
            element.getAttribute("data-i18n-title");

        if (titleKey) {
            element.setAttribute(
                "title",
                getKeyedTranslation(titleKey)
            );
        }

        if (element.tagName === "INPUT") {
            var inputType =
                String(element.type || "").toLowerCase();

            if (inputType === "submit" ||
                inputType === "button" ||
                inputType === "reset") {
                var translatedValue =
                    getExactTranslation(element.value);

                if (translatedValue) {
                    element.value = translatedValue;
                }
            }
        }

        if (element.tagName === "OPTION") {
            var translatedOption =
                getExactTranslation(element.textContent);

            if (translatedOption) {
                element.textContent =
                    translatedOption;
            }
        }
    }

    function translateTree(root) {
        if (!root || isApplyingTranslations) {
            return;
        }

        isApplyingTranslations = true;

        try {
            if (root.nodeType === 1) {
                if (root.hasAttribute("data-i18n")) {
                    translateElementByKey(root);
                }

                translateElementAttributes(root);
            }

            var keyedElements =
                root.querySelectorAll
                    ? root.querySelectorAll("[data-i18n]")
                    : [];

            for (var keyIndex = 0;
                 keyIndex < keyedElements.length;
                 keyIndex++) {
                translateElementByKey(
                    keyedElements[keyIndex]
                );
            }

            var attributeElements =
                root.querySelectorAll
                    ? root.querySelectorAll(
                        "[placeholder], [data-i18n-placeholder], " +
                        "[data-i18n-title], input[type='submit'], " +
                        "input[type='button'], input[type='reset'], option"
                    )
                    : [];

            for (var attributeIndex = 0;
                 attributeIndex < attributeElements.length;
                 attributeIndex++) {
                translateElementAttributes(
                    attributeElements[attributeIndex]
                );
            }

            var walker = document.createTreeWalker(
                root,
                window.NodeFilter.SHOW_TEXT,
                null,
                false
            );

            var textNode;

            while ((textNode = walker.nextNode())) {
                replaceTextNode(textNode);
            }
        } finally {
            isApplyingTranslations = false;
        }
    }

    function translateDocumentTitle() {
        var cleanTitle = normalizeText(document.title);
        var languageDictionary =
            titleTranslations[currentLanguage] || {};
        var translatedTitle =
            languageDictionary[cleanTitle];

        if (translatedTitle) {
            document.title = translatedTitle;
        }
    }

    function injectStyles() {
        if (document.getElementById(
                "appLanguageStyles")) {
            return;
        }

        var style = document.createElement("style");
        style.id = "appLanguageStyles";

        style.textContent = [
            ".app-language-trigger{",
            "display:flex;align-items:center;justify-content:flex-start;",
            "gap:0;border:0;cursor:pointer;font:inherit;text-align:left;",
            "}",
            ".nav .app-language-trigger{",
            "width:100%;height:47px;padding:0 14px;border-radius:11px;",
            "color:#5c7481;background:transparent;white-space:nowrap;",
            "}",
            ".nav .app-language-trigger:hover{",
            "color:#1f709d;background:#edf7fb;",
            "}",
            ".nav .app-language-trigger i{",
            "width:32px;text-align:center;",
            "}",
            ".nav .app-language-trigger .app-language-label{",
            "margin-left:10px;opacity:0;font-size:14px;font-weight:700;",
            "transition:opacity .15s;",
            "}",
            ".side.open .nav .app-language-trigger .app-language-label{",
            "opacity:1;",
            "}",
            ".mainMenu .app-language-trigger{",
            "width:100%;padding:8px 16px;color:inherit;background:transparent;",
            "}",
            ".app-language-top-trigger{",
            "min-height:38px;padding:0 12px;border:1px solid #d2e0e7;",
            "border-radius:9px;color:#1f709d;background:#fff;font-weight:700;",
            "}",
            ".app-language-floating-trigger{",
            "position:fixed;top:16px;right:16px;z-index:9990;",
            "width:44px;height:44px;justify-content:center;border:1px solid #d2e0e7;",
            "border-radius:12px;color:#1f709d;background:#fff;",
            "box-shadow:0 10px 30px rgba(30,70,92,.15);",
            "}",
            ".app-language-modal-backdrop{",
            "position:fixed;inset:0;z-index:10000;display:none;",
            "align-items:center;justify-content:center;padding:18px;",
            "background:rgba(17,39,52,.52);backdrop-filter:blur(3px);",
            "}",
            ".app-language-modal-backdrop.open{display:flex;}",
            ".app-language-modal{",
            "width:min(440px,100%);overflow:hidden;border:1px solid #d6e3e9;",
            "border-radius:18px;background:#fff;",
            "box-shadow:0 28px 90px rgba(15,47,65,.32);",
            "}",
            ".app-language-modal-header{",
            "display:flex;align-items:flex-start;justify-content:space-between;",
            "gap:16px;padding:21px 22px 17px;border-bottom:1px solid #dce5e9;",
            "background:#f8fbfc;",
            "}",
            ".app-language-modal-title{",
            "margin:0;color:#174e6b;font-size:21px;",
            "}",
            ".app-language-modal-subtitle{",
            "margin:6px 0 0;color:#6b7b85;font-size:13px;line-height:1.45;",
            "}",
            ".app-language-modal-close{",
            "display:inline-flex;align-items:center;justify-content:center;",
            "width:35px;height:35px;border:1px solid #d4e0e6;border-radius:9px;",
            "cursor:pointer;color:#536b78;background:#fff;font-size:17px;",
            "}",
            ".app-language-options{",
            "display:grid;grid-template-columns:1fr 1fr;gap:12px;padding:22px;",
            "}",
            ".app-language-option{",
            "position:relative;display:flex;align-items:center;gap:12px;",
            "min-height:78px;padding:14px;border:1px solid #cad9e1;",
            "border-radius:13px;cursor:pointer;color:#344a58;background:#fff;",
            "text-align:left;",
            "}",
            ".app-language-option:hover{",
            "border-color:#78aac3;background:#f6fbfd;",
            "}",
            ".app-language-option.selected{",
            "border-color:#1f709d;box-shadow:0 0 0 3px rgba(31,112,157,.12);",
            "}",
            ".app-language-code{",
            "display:inline-flex;align-items:center;justify-content:center;",
            "width:42px;height:42px;border-radius:11px;color:#fff;",
            "background:#1f709d;font-size:14px;font-weight:800;",
            "}",
            ".app-language-name{display:block;font-size:15px;font-weight:800;}",
            ".app-language-native{",
            "display:block;margin-top:4px;color:#6b7b85;font-size:12px;",
            "}",
            ".app-language-check{",
            "position:absolute;top:9px;right:9px;display:none;color:#1f709d;",
            "}",
            ".app-language-option.selected .app-language-check{display:block;}",
            "@media(max-width:1500px),(max-height:850px){",
            ".nav .app-language-trigger{height:38px;padding:0 9px;border-radius:8px;}",
            ".nav .app-language-trigger i{width:27px;font-size:15px;}",
            ".nav .app-language-trigger .app-language-label{",
            "margin-left:7px;font-size:12px;",
            "}",
            ".app-language-modal{width:min(400px,100%);}",
            ".app-language-modal-header{padding:16px 17px 14px;}",
            ".app-language-modal-title{font-size:18px;}",
            ".app-language-options{padding:16px;}",
            "}",
            "@media(max-width:520px){",
            ".app-language-options{grid-template-columns:1fr;}",
            "}"
        ].join("");

        document.head.appendChild(style);
    }

    function createModal() {
        if (document.getElementById(
                "appLanguageModalBackdrop")) {
            return;
        }

        var backdrop = document.createElement("div");
        backdrop.id = "appLanguageModalBackdrop";
        backdrop.className =
            "app-language-modal-backdrop";
        backdrop.setAttribute("role", "presentation");
        backdrop.setAttribute(
            "data-app-language-ignore",
            "true"
        );

        backdrop.innerHTML =
            '<section class="app-language-modal" ' +
                    'role="dialog" aria-modal="true" ' +
                    'aria-labelledby="appLanguageModalTitle">' +
                '<header class="app-language-modal-header">' +
                    '<div>' +
                        '<h2 id="appLanguageModalTitle" ' +
                            'class="app-language-modal-title">' +
                            'Choose language' +
                        '</h2>' +
                        '<p id="appLanguageModalSubtitle" ' +
                            'class="app-language-modal-subtitle">' +
                            'Select the language used across the application.' +
                        '</p>' +
                    '</div>' +
                    '<button type="button" ' +
                        'class="app-language-modal-close" ' +
                        'aria-label="Close" ' +
                        'data-app-language-close="true">' +
                        '<i class="fa-solid fa-xmark" aria-hidden="true"></i>' +
                    '</button>' +
                '</header>' +
                '<div class="app-language-options">' +
                    createLanguageOptionHtml(
                        "en",
                        "EN",
                        "English",
                        "English"
                    ) +
                    createLanguageOptionHtml(
                        "fr",
                        "FR",
                        "French",
                        "Fran\u00E7ais"
                    ) +
                '</div>' +
            '</section>';

        document.body.appendChild(backdrop);

        backdrop.addEventListener(
            "click",
            function (event) {
                if (event.target === backdrop ||
                    event.target.closest(
                        "[data-app-language-close]"
                    )) {
                    closeModal();
                    return;
                }

                var option =
                    event.target.closest(
                        "[data-app-language-option]"
                    );

                if (!option) {
                    return;
                }

                setLanguage(
                    option.getAttribute(
                        "data-app-language-option"
                    ),
                    true
                );

                closeModal();
            }
        );

        document.addEventListener(
            "keydown",
            function (event) {
                if (event.key === "Escape" &&
                    backdrop.classList.contains("open")) {
                    closeModal();
                }
            }
        );
    }

    function createLanguageOptionHtml(
        language,
        code,
        englishName,
        nativeName) {

        return (
            '<button type="button" ' +
                'class="app-language-option" ' +
                'data-app-language-option="' +
                    language +
                '">' +
                '<span class="app-language-code">' +
                    code +
                '</span>' +
                '<span>' +
                    '<span class="app-language-name">' +
                        englishName +
                    '</span>' +
                    '<span class="app-language-native">' +
                        nativeName +
                    '</span>' +
                '</span>' +
                '<i class="fa-solid fa-circle-check ' +
                    'app-language-check" aria-hidden="true"></i>' +
            '</button>'
        );
    }

    function createTrigger() {
        var existingTriggers =
            document.querySelectorAll(
                "[data-app-language-trigger]"
            );

        if (existingTriggers.length > 0) {
            bindExistingTriggers(existingTriggers);
            updateTriggerLabels();
            return;
        }

        var trigger = document.createElement("button");
        trigger.type = "button";
        trigger.className = "app-language-trigger";
        trigger.setAttribute(
            "data-app-language-trigger",
            "true"
        );
        trigger.innerHTML =
            '<i class="fa-solid fa-language" aria-hidden="true"></i>' +
            '<span class="app-language-label"></span>';

        var navigation =
            document.querySelector(".nav");

        if (navigation) {
            navigation.appendChild(trigger);
        } else {
            var legacyNavigation =
                document.querySelector(".mainMenu");

            if (legacyNavigation) {
                legacyNavigation.appendChild(trigger);
            } else {
                var topbar =
                    document.querySelector(
                        ".toolbar-actions, .actions, .topbar"
                    );

                if (topbar) {
                    trigger.classList.add(
                        "app-language-top-trigger"
                    );

                    topbar.appendChild(trigger);
                } else {
                    trigger.classList.add(
                        "app-language-floating-trigger"
                    );

                    trigger.setAttribute(
                        "title",
                        "Language"
                    );

                    trigger.innerHTML =
                        '<i class="fa-solid fa-language" aria-hidden="true"></i>';

                    document.body.appendChild(trigger);
                }
            }
        }

        bindExistingTriggers([trigger]);
        updateTriggerLabels();
    }

    function bindExistingTriggers(triggers) {
        for (var index = 0;
             index < triggers.length;
             index++) {
            var trigger = triggers[index];

            if (trigger.getAttribute(
                    "data-app-language-bound") === "true") {
                continue;
            }

            trigger.setAttribute(
                "data-app-language-bound",
                "true"
            );

            trigger.addEventListener(
                "click",
                function (event) {
                    event.preventDefault();
                    openModal();
                }
            );
        }
    }

    function updateTriggerLabels() {
        var triggers =
            document.querySelectorAll(
                "[data-app-language-trigger]"
            );

        for (var index = 0;
             index < triggers.length;
             index++) {
            var trigger = triggers[index];
            var label =
                trigger.querySelector(
                    ".app-language-label, " +
                    "#languageNavText, " +
                    "[data-app-language-label]"
                );

            if (label) {
                label.textContent =
                    getKeyedTranslation(
                        "nav.language"
                    ) +
                    " \u00B7 " +
                    currentLanguage.toUpperCase();
            }

            trigger.setAttribute(
                "title",
                getKeyedTranslation(
                    "nav.language"
                )
            );
        }
    }

    function updateModalLanguage() {
        var modalTitle =
            document.getElementById(
                "appLanguageModalTitle"
            );

        var modalSubtitle =
            document.getElementById(
                "appLanguageModalSubtitle"
            );

        if (modalTitle) {
            modalTitle.textContent =
                currentLanguage === "fr"
                    ? "Choisir la langue"
                    : "Choose language";
        }

        if (modalSubtitle) {
            modalSubtitle.textContent =
                currentLanguage === "fr"
                    ? "S\u00E9lectionnez la langue utilis\u00E9e dans toute l\u2019application."
                    : "Select the language used across the application.";
        }

        var englishOption =
            document.querySelector(
                '[data-app-language-option="en"]'
            );

        var frenchOption =
            document.querySelector(
                '[data-app-language-option="fr"]'
            );

        if (englishOption) {
            var englishName =
                englishOption.querySelector(
                    ".app-language-name"
                );

            if (englishName) {
                englishName.textContent =
                    currentLanguage === "fr"
                        ? "Anglais"
                        : "English";
            }
        }

        if (frenchOption) {
            var frenchName =
                frenchOption.querySelector(
                    ".app-language-name"
                );

            if (frenchName) {
                frenchName.textContent =
                    currentLanguage === "fr"
                        ? "Fran\u00E7ais"
                        : "French";
            }
        }
    }

    function updateSelectedOption() {
        var options =
            document.querySelectorAll(
                "[data-app-language-option]"
            );

        for (var index = 0;
             index < options.length;
             index++) {
            var option = options[index];

            option.classList.toggle(
                "selected",
                option.getAttribute(
                    "data-app-language-option"
                ) === currentLanguage
            );
        }
    }

    function applyTranslations() {
        document.documentElement.lang =
            currentLanguage;

        translateDocumentTitle();
        translateTree(document.body);
        updateTriggerLabels();
        updateModalLanguage();
        updateSelectedOption();
    }

    function setLanguage(
        language,
        notifyListeners) {

        if (supportedLanguages.indexOf(language) < 0) {
            return;
        }

        currentLanguage = language;
        persistLanguage(language);
        applyTranslations();

        if (notifyListeners !== false) {
            window.dispatchEvent(
                new CustomEvent(
                    "appLanguageChanged",
                    {
                        detail: {
                            language: currentLanguage
                        }
                    }
                )
            );
        }
    }

    function openModal() {
        createModal();
        updateModalLanguage();
        updateSelectedOption();

        var backdrop =
            document.getElementById(
                "appLanguageModalBackdrop"
            );

        if (!backdrop) {
            return;
        }

        backdrop.classList.add("open");
        document.body.style.overflow = "hidden";

        var selectedOption =
            backdrop.querySelector(
                ".app-language-option.selected"
            );

        if (selectedOption) {
            selectedOption.focus();
        }
    }

    function closeModal() {
        var backdrop =
            document.getElementById(
                "appLanguageModalBackdrop"
            );

        if (!backdrop) {
            return;
        }

        backdrop.classList.remove("open");
        document.body.style.overflow = "";
    }

    function observeDynamicContent() {
        if (!window.MutationObserver ||
            mutationObserver) {
            return;
        }

        mutationObserver =
            new MutationObserver(
                function (mutations) {
                    if (isApplyingTranslations) {
                        return;
                    }

                    for (var mutationIndex = 0;
                         mutationIndex < mutations.length;
                         mutationIndex++) {
                        var mutation =
                            mutations[mutationIndex];

                        for (var nodeIndex = 0;
                             nodeIndex < mutation.addedNodes.length;
                             nodeIndex++) {
                            var node =
                                mutation.addedNodes[nodeIndex];

                            if (node.nodeType === 1) {
                                translateTree(node);
                            } else if (node.nodeType === 3) {
                                replaceTextNode(node);
                            }
                        }
                    }
                }
            );

        mutationObserver.observe(
            document.body,
            {
                childList: true,
                subtree: true
            }
        );
    }

    function initialize() {
        injectStyles();
        createModal();
        createTrigger();
        setLanguage(
            currentLanguage,
            false
        );
        observeDynamicContent();

        /*
         * Let page-specific scripts, graphs and modals redraw themselves
         * using the globally selected language.
         */
        window.dispatchEvent(
            new CustomEvent(
                "appLanguageChanged",
                {
                    detail: {
                        language: currentLanguage
                    }
                }
            )
        );
    }

    window.AppLanguage = {
        openModal: openModal,
        closeModal: closeModal,
        setLanguage: function (language) {
            setLanguage(language, true);
        },
        getLanguage: function () {
            return currentLanguage;
        },
        translate: getKeyedTranslation,
        apply: applyTranslations
    };

    if (document.readyState === "loading") {
        document.addEventListener(
            "DOMContentLoaded",
            initialize
        );
    } else {
        initialize();
    }
})(window, document);
