export const generateLocationNames = (numLocations) => {
  const alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
  const locationNames = [];

  for (let i = 0; i < numLocations; i++) {
    let name = "";
    let temp = i;

    while (temp >= 0) {
      name = alphabet[temp % 26] + name;
      temp = Math.floor(temp / 26) - 1;
    }

    locationNames.push(name);
  }

  return locationNames;
};
