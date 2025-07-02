import { useEffect, useState } from "react";

const TITLES = [
  "Join NoorSphere and showcase your skills while connecting with like-minded developers.",
  "NoorSphere—where engineers and developers unite to innovate, collaborate, and grow.",
  "Supercharge your career by building a powerful network with fellow developers on NoorSphere!",
];

const LandingTitle = () => {
  const [titleIndex, setTitleIndex] = useState(0);
  const [fadeIn, setFadeIn] = useState(true);

  useEffect(() => {
    const fadeOutTimer = setTimeout(() => setFadeIn(false), 2000);
    const changeTitleTimer = setTimeout(() => {
      setTitleIndex((prev) => (prev + 1) % TITLES.length);
      setFadeIn(true);
    }, 4000);

    return () => {
      clearTimeout(fadeOutTimer);
      clearTimeout(changeTitleTimer);
    };
  }, [titleIndex]);

  return <p className={fadeIn ? "title-fade-in" : "title-fade-out"}>{TITLES[titleIndex]}</p>;
};

export default LandingTitle;
