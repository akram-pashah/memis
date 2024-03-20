const unitCost = document.getElementById("unitCost");
const Q1Target = document.getElementById("Q1Target");
const Q1Budget = document.getElementById("Q1Budget");
const Q2Target = document.getElementById("Q2Target");
const Q2Budget = document.getElementById("Q2Budget");
const Q3Target = document.getElementById("Q3Target");
const Q3Budget = document.getElementById("Q3Budget");
const Q4Target = document.getElementById("Q4Target");
const Q4Budget = document.getElementById("Q4Budget");
const comparativeTarget = document.getElementById("comparativeTarget");
const budgetAmount = document.getElementById("budgetAmount");
unitCost.oninput = calculateQ1Budget;
Q1Target.oninput = calculateQ1Budget;
Q2Target.oninput = calculateQ2Budget;
Q3Target.oninput = calculateQ3Budget;
Q4Target.oninput = calculateQ4Budget;
const num1 = parseFloat(unitCost.value);
function calcBudget() {
    const num2 = 0;
    const num3 = 0;
    const num4 = 0;
    const num5 = 0;
    if (Q1Target.length > 0) { num2 = parseFloat(Q1Target.value); }
    if (Q2Target.length > 0) { num3 = parseFloat(Q2Target.value); }
    if (Q3Target.length > 0) {
        num4 = parseFloat(Q3Target.value);
    }
    if (Q4Target.length > 0) {
        num5 = parseFloat(Q4Target.value);
    }
    let tot = num2 + num3 + num4 + num5;
    comparativeTarget.value = tot.toFixed(2);
    budgetAmount.value = (tot * num1);
}
function calculateQ1Budget() {
    const num2 = parseFloat(Q1Target.value);
    const sum = num1 * num2;
    Q1Budget.value = sum.toFixed(2);
    calcBudget();
}
function calculateQ2Budget() {
    const num3 = parseFloat(Q2Target.value);
    const sum = num1 * num3;
    Q2Budget.value = sum.toFixed(2);
    calcBudget();
}
function calculateQ3Budget() {
    const num4 = parseFloat(Q3Target.value);
    const sum = num1 * num4;
    Q3Budget.value = sum.toFixed(2);
    calcBudget();
}
function calculateQ4Budget() {

    const num5 = parseFloat(Q4Target.value);
    const sum = num1 * num5;
    Q4Budget.value = sum.toFixed(2);
    calcBudget();
}
