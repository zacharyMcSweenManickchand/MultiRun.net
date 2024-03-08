#!/bin/bash

# Function to generate Lorem Ipsum text
generate_lorem_ipsum() {
    echo "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
    echo ""
}

# Check if two arguments are provided
if [ "$#" -ne 2 ]; then
    echo "Usage: $0 <paragraphs> <wait_time>"
    exit 1
fi

paragraphs="$1"
wait_time="$2"

# Iterate over the number of paragraphs specified
for ((i=1; i<=$paragraphs; i++)); do
    echo "Paragraph $i:"
    generate_lorem_ipsum
    sleep "$wait_time"
done
